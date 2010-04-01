using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Ionic.Zip;

namespace Crabwise.PDFtoEPUB
{
    public class EPUBGenerator
    {
        public static void GenerateEPUB(string inputFile, string outputFile, ConversionOptions options)
        {
            //generate mimetype
            //generate META-INF
                //container.xml
            //generate OEBPS
                //images
                //Content.opf
                //toc.ncx
                //xhtml
        }
        public static ConversionOptions ScanPDFContent(string inputFilePath)
        {
            string html = HtmlConversion.GenerateHTMLFromFile(inputFilePath);
            ConversionOptions options = MetadataGenerator.FindMetaData(html);
            return options;
        }
    }
}
