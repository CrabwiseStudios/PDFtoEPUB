using System;
using System.Collections.Generic;
using System.Text;

namespace Crabwise.PDFtoEPUB
{
    public class TOCFileGenerator
    {
        public static byte[] GetTOCFile(ConversionOptions options)
        {
            StringBuilder tocBuilder = new StringBuilder();

            tocBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");

            //ncx
            tocBuilder.AppendLine("<ncx xmlns=\"http://www.daisy.org/z3986/2005/ncx/\" version=\"2005-1\">");

            //ncx/head
            tocBuilder.AppendLine("<head>");

            //ncx/head/meta(s)
            tocBuilder.AppendFormat("<meta name=\"dtb:uid\" content=\"{0}\"/>", options.ID);
            tocBuilder.AppendFormat("<meta name=\"dtb:depth\" content=\"{0}\"/>", 1);
            tocBuilder.AppendFormat("<meta name=\"dtb:totalPageCount\" content=\"{0}\"/>", 0);
            tocBuilder.AppendFormat("<meta name=\"dtb:maxPageNumber\" content=\"{0}\"/>", 0);
            //end /ncx/head/meta(s)

            //end /ncx/head
            tocBuilder.AppendLine("</head>");

            //ncx/docTitle
            tocBuilder.AppendLine("<docTitle>");

            //ncx/docTitle/text & end
            tocBuilder.AppendFormat("<text>{0}</text>", options.Title);

            //end /ncx/docTitle/text
            tocBuilder.AppendLine("</docTitle>");

            //ncx/navMap
            tocBuilder.AppendLine("<navMap>");
            

            //ncx/navMap/navPoint(s)
            //title page
            tocBuilder.AppendLine(GenerateNavPoint("Title Page", 1, "title_page.xhtml"));
            foreach (Chapter chapter in options.Chapters)
            {
                //chapters
                //need the +1 because the title page isn't included in the chapter list
                tocBuilder.AppendLine(GenerateNavPoint(chapter.ChapterTitle, chapter.ChapterNumber + 1, chapter.EmbeddedTitle));
            }
            //end /ncx/navMap/navPoint(s)

            //end /ncx/navMap
            tocBuilder.AppendLine("</navMap>");
            //end /ncx
            tocBuilder.AppendLine("</ncx>");

            //return the xml in ASCII encoding
            return Encoding.ASCII.GetBytes(tocBuilder.ToString());
        }

        static string GenerateNavPoint(string Title, int PlayOrder, string ContentSrc)
        {
            StringBuilder navPointBuilder = new StringBuilder();

            //navPoint
            navPointBuilder.AppendFormat("<navPoint id=\"{0}\" playOrder=\"{1}\">", Title.ToLower(), PlayOrder.ToString());
            //navPoint/navLabel
            navPointBuilder.AppendLine("<navLabel>");
            //navPoint/navLabel/text & end
            navPointBuilder.AppendFormat("<text>{0}</text>", Title);
            //end /navPoint/navLabel
            navPointBuilder.AppendLine("</navLabel>");
            //navPoint/content & end
            navPointBuilder.AppendFormat("<content src=\"{0}\"/>", ContentSrc);
            //end /navPoint
            navPointBuilder.AppendLine("</navPoint>");

            return navPointBuilder.ToString();
        }
    }
}
