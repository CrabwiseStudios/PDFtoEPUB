using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Crabwise.PDFtoEPUB
{
    class HtmlSplitter
    {
        [Flags]
        private enum LineType
        {
            None = 0,
            CloseBefore = 1,
            CloseEnd = 2,
            PreFormatted = 4
        }

        private string _WorkingDir;
        private string _HtmlHeader;
        private string _HtmlFooter;
        private HtmlWriter _Writer;

        private ConversionOptions _CurrentOptions;
        private const string _DocumentOutline = "<a name=\"outline\"></a><h1>document outline</h1>";
        private const string _BodyPattern = "<body(.|\n)*?>";
        private const string _PagePattern = "<hr(.|\n)*?>";
        private const string _BodyStartTag = "<html  xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head><title></title><link href=\"stylesheet.css\" type=\"text/css\" rel=\"stylesheet\"/>\n</head><body>\n";
        private const string _BodyEndTag = "</body>\n</html>\n";
        private Dictionary<string, string> _ReferenceList = new Dictionary<string, string>();

        public void SplitFile(ConversionOptions options)
        {
            _CurrentOptions = options;
            _WorkingDir = "";
            _HtmlFooter = _BodyEndTag;
            ProcessFile(_CurrentOptions.InputFile);
        }

        private string GetWorkingDir(string fileName)
        {
            string fullPath = Path.GetFullPath(fileName);
            return Path.GetDirectoryName(fullPath);
        }

        private void ProcessFile(string fileName)
        {
            try
            {
                using (StreamReader reader = new StreamReader(fileName))
                {
                    PatternSearcher searcher = new PatternSearcher(reader, 8192);
                    BuildHeader(searcher);
                    _Writer = new HtmlWriter(_HtmlHeader, _HtmlFooter);
                    GetPages(searcher);
                }

            }
            catch (Exception)
            {
               
            }
        }

        private void BuildHeader(PatternSearcher searcher)
        {
            string header = GetHeader(searcher);
            _HtmlHeader = _BodyStartTag;
        }

        private string GetHeader(PatternSearcher searcher)
        {
            PatternFound found = searcher.NextPattern(_BodyPattern);
            if (found.EndOfStreamReached == true)
            {
                throw new ApplicationException("Tag <Body> is not found.");
            }

            return found.TextBeforePattern;
        }

        private void GetPages(PatternSearcher searcher)
        {
            for (PatternFound found = searcher.NextPattern(_PagePattern); found.EndOfStreamReached == false; found = searcher.NextPattern(_PagePattern))
            {
                string page = ProcessHtmlPage(found.TextBeforePattern);
                if (string.IsNullOrEmpty(page) == false)
                {
                    _Writer.AddPage(page);
                }
            }
        }

        private string ProcessHtmlPage(string page)
        {
            page = TrimLeadingSpaces(page);

            int offset = SkipPageLink(page);
            if ((_CurrentOptions.StripHeader != null) && (_CurrentOptions.StripHeader.Enable))
            {
                page = TrimPageHeader(page, _CurrentOptions.StripHeader, offset);
            }
            if ((_CurrentOptions.StripFooter != null) && (_CurrentOptions.StripFooter.Enable))
            {
                page = TrimPageFooter(page, _CurrentOptions.StripFooter);
            }
            page = MakeXhtmlConformant(page);
            page = ConvertLines(page);
            return page;
                      
        }


        private string ConvertLines(string page)
        {
            StringBuilder sb = new StringBuilder(page.Length + 1024);
            string[] lines = page.Split(new string[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries);
            lines[0] = SkipPageLink(lines[0], sb);
            TrimLines(lines);

            bool flagOpened = false;
            for (int count = 0; count < lines.Length; count++)
            {
                string nextLine = (count < lines.Length - 2) ? lines[count + 1] : null;
                LineType type = DetermineLineType(ref lines[count], nextLine);
                flagOpened = AddLinePrefixTags(type, flagOpened, sb);
                sb.Append(lines[count]);
                flagOpened = AddLinePostfixTags(type, flagOpened, sb, count, lines.Length);
            }
            return sb.ToString();
        }


        private bool AddLinePrefixTags(LineType type, bool flagOpened, StringBuilder sb)
        {
            if (((type & LineType.CloseBefore) != 0) || (flagOpened == false))
            {
                sb.Append("<p>");
                flagOpened = true;
            }
            if ((type & LineType.PreFormatted) != 0)
            {
                sb.Append("<pre>");
            }
            return flagOpened;
        }

        private bool AddLinePostfixTags(LineType type, bool flagOpened, StringBuilder sb, int count, int linesLength)
        {
            if ((type & LineType.PreFormatted) != 0)
            {
                sb.Append("</pre>");
            }
            if (((type & LineType.CloseEnd) != 0) || (count == linesLength - 1))
            {
                sb.Append("</p>");
                flagOpened = false;
            }
            return flagOpened;
        }

        private void TrimLines(string[] lines)
        {
            for (int index = 0; index < lines.Length; index++)
            {
                lines[index] = lines[index].Trim(new char[] { '\n', '\r' });
            }
        }

        private LineType DetermineLineType(ref string line, string nextLine)
        {
            LineType type = LineType.None;

            if (line.Length == 0)
            {
                return type;
            }
            if ((line[0] == ' ') || (nextLine != null && nextLine[0] == ' '))
            {
                return LineType.PreFormatted;
            }
            if (line[0] == '<')
            {
                type |= LineType.CloseBefore;
            }
            if ((line[line.Length - 1] == '>') || (line.Length < 30))
            {
                type |= LineType.CloseEnd;
            }
            return type;
        }

        private string SkipPageLink(string line, StringBuilder sb)
        {
            int offset = SkipPageLink(line);
            sb.Append(line.Substring(0, offset));
            return line.Substring(offset);
        }

        private string TrimLeadingSpaces(string page)
        {
            return page.TrimStart(new char[] { '\n', '\r' });
        }

        private int SkipPageLink(string page)
        {
            Match match = Regex.Match(page, "<a.*?></a>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if ((match != null) && (match.Success == true) && (match.Index == 0))
            {
                return match.Length;
            }
            return 0;
        }

        private string TrimPageHeader(string page, Pattern headerPattern, int offset)
        {
            int beginning = SkipLeadingImage(page, offset);
            string header = GetFirstLines(headerPattern.LineCount, page, beginning);
            if (string.IsNullOrEmpty(header) == false)
            {
                bool flagMatch = StringIsMatch(header, headerPattern.RegEx);
                if (flagMatch)
                {
                    page = page.Remove(beginning, header.Length);
                }
            }
            return page;
        }


        private int SkipLeadingImage(string page, int beginning)
        {
            if (beginning > page.Length - 6)
            {
                return beginning;
            }

            string imgTag = page.Substring(beginning, 4).ToLower();
            if (string.Compare(imgTag, "<img") == 0)
            {
                int offset = page.IndexOf('\n', beginning);
                if (offset <= 0)
                {
                    throw new ApplicationException("End of line is not found");
                }
                return offset + 1;
            }
            return beginning;
        }

        private string GetFirstLines(int count, string page, int start)
        {
            int offset = start;
            while ((count > 0) && (offset < page.Length - 1))
            {
                offset = page.IndexOf('\n', offset);
                if (offset == -1)
                {
                    return null;
                }
                count -= 1;
                offset += 1;
            }
            return page.Substring(start, offset - start);
        }

        private bool StringIsMatch(string matchString, string regEx)
        {
            Regex rx = new Regex(regEx, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Match match = rx.Match(matchString);
            if ((match != null)
                && (match.Success == true)
                && (match.Index == 0))
            {
                return true;
            }
            return false;
        }

        private string TrimPageFooter(string page, Pattern pattern)
        {
            string footer = GetLastLines(pattern.LineCount, page);
            if (string.IsNullOrEmpty(footer) == false)
            {
                bool flagMatch = StringIsMatch(footer, pattern.RegEx);
                if (flagMatch)
                {
                    page = page.Remove(page.Length - footer.Length, footer.Length);
                }
            }
            return page;
        }

        private string GetLastLines(int count, string page)
        {
            int offset = page.Length;
            while (count > 0)
            {
                offset = page.LastIndexOf('\n', offset - 2);
                if (offset == -1)
                {
                    return null;
                }
                count -= 1;
            }
            return page.Substring(offset + 1);
        }

        private string MakeXhtmlConformant(string page)
        {
            page = page.Replace("<A ", "<a ");
            page = page.Replace("<IMG ", "<img ");
            //page = page.Replace( "<br>", "<br/>" );
            page = UpdateATag(page);
            page = UpdateImgTag(page);
            return page;
        }

        private string UpdateImgTag(string page)
        {
            int offset = 0;
            do
            {
                offset = page.IndexOf("<img", offset);
                if (offset != -1)
                {
                    offset = page.IndexOf('>', offset);
                    if (offset != -1)
                    {
                        page = page.Insert(offset, " alt=\"\" /");
                    }
                }
            } while (offset >= 0);
            return page;
        }

        private string UpdateATag(string page)
        {
            List<int> locationList = new List<int>();

            MatchCollection matchCollection = Regex.Matches(page, "<a name=(?<href>.*?)>", RegexOptions.Multiline);
            foreach (Match match in matchCollection)
            {
                string val = match.Groups["href"].Value;
                string newALink = string.Format("<a id=\"{0}\">", val);

                page = page.Remove(match.Index, match.Length);
                page = page.Insert(match.Index, newALink);

                _ReferenceList.Add(val, string.Format("{0}#{1}", _Writer.CurrentFilename, val));
            }

            return InsertQuotes(page, locationList);
        }

        private string InsertQuotes(string page, List<int> ints)
        {
            for(int i = ints.Count -1; i >= 0; i--)
            {
                page = page.Insert(ints[i], "\"");
            }
            return page;
        }


        private bool CheckIsPrintable(string title)
        {
            int printable = 0;
            foreach (char c in title)
            {
                if (Char.IsLetterOrDigit(c) || Char.IsPunctuation(c) || char.IsWhiteSpace(c))
                {
                    printable += 1;
                }
            }
            return printable > (title.Length / 2);
        }

        private string GetNewHref(string href)
        {
            string[] parts = href.Split(new char[] { '#' }, 2);
            if ((parts.Length == 2) && _ReferenceList.ContainsKey(parts[1]))
            {
                return _ReferenceList[parts[1]];
            }
            return href;
        }

    }
}
