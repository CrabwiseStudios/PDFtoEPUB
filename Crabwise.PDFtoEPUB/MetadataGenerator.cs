using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Crabwise.PDFtoEPUB
{
    public class MetadataGenerator
    {
        public static ConversionOptions FindMetaData(string html)
        {
            ConversionOptions returnable = new ConversionOptions();
            returnable.Title = ParseTag(html, "Title");
            returnable.Author = ParseMeta(html, "author");
            DateTime publish = DateTime.MinValue;
            DateTime.TryParse(ParseMeta(html, "date"), out publish);
            returnable.PublishDate = publish;

            return returnable;

        }
        private static string ParseTag(string html, string tag)
        {
            string returnString = string.Empty;
            string regex = string.Format("<{0}.*?>(?<info>.*?)</{0}", tag);
            Match result = Regex.Match(html, regex, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (result.Success)
                returnString = result.Groups["info"].Value;
            return returnString;
        }
        private static string ParseMeta(string html, string tag)
        {
            string returnString = string.Empty;
            string regex = string.Format("<META name-\"{0}\" content=\"(?<content>.*?)\">", tag);
            Match result = Regex.Match(html, regex, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (result.Success)
                returnString = result.Groups["content"].Value;
            return returnString;
        }
    }
}
