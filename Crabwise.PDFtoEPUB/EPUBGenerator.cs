using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Ionic.Zip;

namespace Crabwise.PDFtoEPUB
{
    public class EPUBGenerator
    {
        private const string _mimetype = "application/epub+zip";
        public static void GenerateEPUB(string inputFile, string outputFile, ConversionOptions options)
        {
            ZipFile file = new ZipFile(outputFile);

            //generate mimetype
            using (Stream mime = new MemoryStream())
            {
                byte[] mimeBytes = System.Text.Encoding.ASCII.GetBytes(_mimetype);
                mime.Write(mimeBytes, 0, mimeBytes.Length);
                file.AddEntry("mimetype", mime);
            }

            //generate META-INF
            file.AddDirectoryByName("META-INF");
            {
                //container.xml
                using (Stream container = new MemoryStream())
                {
                    byte[] containerBytes = System.Text.Encoding.ASCII.GetBytes(Crabwise.PDFtoEPUB.Properties.Resources.container);
                    container.Write(containerBytes, 0, containerBytes.Length);
                    file.AddEntry("META-INF\\container.xml", container);
                }
            }
            //generate OEBPS
            file.AddDirectoryByName("OEBPS");
            {
                //images
                file.AddDirectoryByName("OEBPS\\IMAGES");
                {
                    //add each image to the directory
                }

                //xhtml chapters
                

                //page-template.xpgt
                byte[] pagetemplateBytes = System.Text.Encoding.ASCII.GetBytes(Properties.Resources.page_template);
                file.AddEntry("OEBPS\\page-template.xpgt", pagetemplateBytes);

                //stylesheet.css
                byte[] stylesheetBytes = System.Text.Encoding.ASCII.GetBytes(Properties.Resources.stylesheet);
                file.AddEntry("OEBPS\\stylesheet.css", stylesheetBytes);

                //title_page.xhtml
                file.AddEntry("OEBPS\\title_page.xhtml", TitlePageGenerator.GetTitlePage(options));

                //Content.opf
                file.AddEntry("OEBPS\\content.opf", ContentFileGenerator.GetContentFile(options));
                
                //toc.ncx
                file.AddEntry("OEBPS\\toc.ncx", TOCFileGenerator.GetTOCFile(options));


            }
        }
        public static ConversionOptions ScanPDFContent(string inputFilePath)
        {
            string html = HtmlConversion.GenerateHTMLFromFile(inputFilePath);
            ConversionOptions options = MetadataGenerator.FindMetaData(html);
            return options;
        }
    }
}
