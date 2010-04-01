using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Crabwise.PDFtoEPUB
{
    public class TOCFileGenerator
    {
        public static byte[] GetTOCFile(ConversionOptions options)
        {
            XmlDocument tocxml = new XmlDocument();

            //ncx, the root element
            tocxml.LoadXml(@"<?xml version=""1.0"" encoding=""UTF-8""?><ncx xmlns=""http://www.daisy.org/z3986/2005/ncx/"" version=""2005-1""></ncx>");
            
            //ncx/head
            XmlElement head = tocxml.CreateElement("head");
            
            //ncx/head/dtb:uid
            XmlElement uid = tocxml.CreateElement("meta");
            uid.SetAttribute("name", "dtb:uid");
            uid.SetAttribute("content", System.Guid.NewGuid().ToString());

            //ncx/head/dtb:depth
            XmlElement depth = tocxml.CreateElement("meta");
            depth.SetAttribute("name", "dtb:depth");
            depth.SetAttribute("content", "1");

            //ncx/head/dtb:totalPageCount
            XmlElement totalPageCount = tocxml.CreateElement("meta");
            totalPageCount.SetAttribute("name", "dtb:totalPageCount");
            totalPageCount.SetAttribute("content", "0");

            //ncx/head/dtb:maxPageNumber
            XmlElement maxPageNumber = tocxml.CreateElement("meta");
            maxPageNumber.SetAttribute("name", "dtb:maxPageNumber");
            maxPageNumber.SetAttribute("content", "0");

            //append the dtb:* elements to the head
            head.AppendChild((XmlNode)uid).AppendChild((XmlNode)depth).AppendChild((XmlNode)totalPageCount).AppendChild((XmlNode)maxPageNumber);

            //append the head to ncx, the root element
            tocxml.DocumentElement.AppendChild((XmlNode)head);

            //ncx/docTitle
            XmlElement docTitle = tocxml.CreateElement("docTitle");
            docTitle.InnerXml = String.Format("<text>{0}</text>", options.Title);

            //append the docTitle element to ncx, the root element
            tocxml.DocumentElement.AppendChild((XmlNode)docTitle);

            //ncx/navMap
            XmlElement navMap = tocxml.CreateElement("navMap");

            foreach (Chapter chapter in options.Chapters)
            {
                //ncx/navMap/navPoint
                XmlElement navPoint = tocxml.CreateElement("navPoint");
                navPoint.SetAttribute("id", chapter.ChapterTitle);
                navPoint.SetAttribute("playOrder", chapter.ChapterNumber.ToString());

                //ncx/navMap/navPoint/navLabel
                XmlElement navLabel = tocxml.CreateElement("navLabel");
                navLabel.InnerXml = String.Format("<text>{0}</text>");

                //ncx/navMap/navPoint/content
                XmlElement content = tocxml.CreateElement("content");
                content.SetAttribute("src", chapter.EmbeddedTitle);

                //append the navLabel element and the content element to the navPoint element for the chapter
                navPoint.AppendChild((XmlNode)navLabel).AppendChild((XmlNode)content);
                
                //apend the navPoint of the chapter to the navMap
                navMap.AppendChild((XmlNode)navPoint);
            }

            //append the navMap to ncx, the root element
            tocxml.DocumentElement.AppendChild((XmlNode)navMap);

            //return the xml in UTF8 encoding
            return System.Text.Encoding.UTF8.GetBytes(tocxml.ToString());
        }
    }
}
