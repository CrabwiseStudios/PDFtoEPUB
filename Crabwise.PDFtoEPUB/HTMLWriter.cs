using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Crabwise.PDFtoEPUB
{
    public class HtmlWriter
    {
        private string _HtmlHeader;
        private string _HtmlFooter;
        private List<string> pages;

        public string CurrentFilename;

        public HtmlWriter(string htmlHeader, string htmlFooter)
        {
            _HtmlHeader = htmlHeader;
            _HtmlFooter = htmlFooter;
            pages = new List<string>();
        }

        public List<string> GetPages()
        {
            return pages;
        }

        public void AddPage(string newPage)
        {
            pages.Add("\n" + _HtmlHeader + "\n" + newPage + "\n" + _HtmlFooter);
        }
    }
}
