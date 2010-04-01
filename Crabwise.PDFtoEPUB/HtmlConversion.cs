using Crabwise.PDFtoEPUB.Commands;

namespace Crabwise.PDFtoEPUB
{
    public static class HtmlConversion
    {
        public static string GenerateHTMLFromFile(string pdfFilePath)
        {
            PdfToHtmlCommand cmd = new PdfToHtmlCommand();
            cmd.PDFFileLocation = pdfFilePath;
            cmd.WriteToStdOut = true;
            cmd.ExchangePDFLinksByHTML = true;
            Crabwise.CommandWrap.CommandStartInfo startinfo = new Crabwise.CommandWrap.CommandStartInfo(new System.Diagnostics.ProcessStartInfo());
            startinfo.WorkingDirectory = System.IO.Directory.GetCurrentDirectory();
            startinfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            cmd.Execute(startinfo);
            return cmd.StandardOutput;
        }
    }
}
