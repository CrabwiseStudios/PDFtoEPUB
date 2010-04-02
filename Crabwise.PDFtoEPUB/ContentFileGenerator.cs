using System;
using System.Collections.Generic;
using System.Text;

namespace Crabwise.PDFtoEPUB
{
    public class ContentFileGenerator
    {
        public static byte[] GetContentFile(ConversionOptions options)
        {
            StringBuilder contentBuilder = new StringBuilder();

            contentBuilder.AppendLine("<?xml version=\"1.0\"?>");

            //package
            contentBuilder.AppendFormat("<package version=\"2.0\" xmlns=\"http://www.idpf.org/2007/opf\" unique-identifier=\"{0}\">", options.ID);
            
            //package/metadata
            contentBuilder.AppendLine("<metadata xmlns:dc=\"http://purl.org/dc/elements/1.1/\" xmlns:opf=\"http://www.idpf.org/2007/opf\">");
            //package/metadata/dc:title & end
            contentBuilder.AppendFormat("<dc:title>{0}</dc:title>", options.Title);
            //package/metadata/dc:creator & end
            contentBuilder.AppendFormat("<dc:creator opf:role=\"aut\">{0}</dc:creator>", options.Author);
            //package/metadata/dc:language & end
            contentBuilder.AppendLine("<dc:language>en-US</dc:language>"); //assuming en-US
            //package/metadata/dc:rights & end
            contentBuilder.AppendLine("<dc:rights>Public Domain</dc:rights>");
            //package/metadata/dc:publishers & end
            contentBuilder.AppendLine("<dc:publisher>Crabwise Studios</dc:publisher>");
            //package/metadata/dc:identifier & end
            contentBuilder.AppendFormat("<dc:identifier id=\"BookId\">urn:uuid:{0}</dc:identifier>", options.ID);
            //end /package/metadata
            contentBuilder.AppendLine("</metadata>");

            //package/manifest
            contentBuilder.AppendLine("<manifest>");

            //package/manifest/item(s)

            //table of contents file
            contentBuilder.AppendLine(GenerateItem("ncx", "toc.ncx", "text-xml"));
            //main stylesheet
            contentBuilder.AppendLine(GenerateItem("style", "stylesheet.css", "text/css"));
            //adobe digital editions page template
            contentBuilder.AppendLine(GenerateItem("pagetemplate", "page-template.xpgt", "application/vnd.adobe-page-template+xml"));
            //title page
            contentBuilder.AppendLine(GenerateItem("titlepage", "title_page.xhtml", "application/xhtml+xml"));
            
            //add the chapters to the manifest
            foreach (Chapter chapter in options.Chapters)
            {
                //example id: chapter01, chapter19
                contentBuilder.AppendLine(GenerateItem("chapter" + chapter.ChapterNumber.ToString().PadLeft(2, '0'), chapter.EmbeddedTitle, "application/xhtml+xml"));
            }

            //add the images to the manifest
            for (int imgi = 0; imgi < options.Images.Count; imgi++)
            {
                //example id: img01, img19
                contentBuilder.AppendLine(GenerateItem("img" + imgi.ToString().PadLeft(3, '0'), options.Images[imgi], "imge/jpeg"));
            }

            //end /package/manifest
            contentBuilder.AppendLine("</manifest>");

            //package/spine
            contentBuilder.AppendLine("<spine toc=\"ncx\">");

            //package/spine/itemref(s)

            //add the title page to the spine
            contentBuilder.AppendLine(GenerateItemRef("titlepage"));

            //add the chapters to the spine
            foreach (Chapter chapter in options.Chapters)
            {
                //example same as above
                contentBuilder.AppendLine(GenerateItemRef("chapter" + chapter.ChapterNumber.ToString().PadLeft(2, '0')));
            }
            //end /package/spine
            contentBuilder.AppendLine("</spine>");

            //end /package
            contentBuilder.AppendLine("</package>");

            //return the xml in ASCII encoding
            return Encoding.ASCII.GetBytes(contentBuilder.ToString());
        }

        static string GenerateItem(string ID, string href, string mimetype)
        {
            //ex: <item id="chapter01" href="ch01.xhtml" media-type="application/xhtml+xml" />
            return String.Format("<item id=\"{0}\" href=\"{1}\" media-type=\"{2}\" />", ID, href, mimetype);
        }

        static string GenerateItemRef(string ID)
        {
            //ex: <itemref idref="ch01" />
            return String.Format("<itemref idref=\"{0}\" />", ID);
        }
    }
}
