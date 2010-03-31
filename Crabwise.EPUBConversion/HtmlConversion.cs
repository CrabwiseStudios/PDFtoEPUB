using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crabwise.EPUBConversion
{
    public static class HtmlConversion
    {
        public static string GenerateHTMLFromFile(string pdfFilePath)
        {
            pdfToHTMLCommand cmd = new pdfToHTMLCommand();
            cmd.PDFFileLocation = pdfFilePath;
            cmd.WriteToStdOut = true;
            cmd.ExchangePDFLinksByHTML = true;
            Crabwise.CommandWrap.Library.CommandStartInfo startinfo = new Crabwise.CommandWrap.Library.CommandStartInfo(new System.Diagnostics.ProcessStartInfo());
            startinfo.WorkingDirectory = System.IO.Directory.GetCurrentDirectory();
            startinfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            cmd.Execute(startinfo);
            return cmd.StandardOutput;
        }
    }
}
