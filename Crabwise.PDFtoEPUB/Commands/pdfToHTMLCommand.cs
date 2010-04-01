using System;

namespace Crabwise.PDFtoEPUB.Commands
{
    using Crabwise.CommandWrap.Library;

    [CommandSyntax("pdftohtml.exe")]
    public class PdfToHtmlCommand : Command
    {

        [ParameterSyntax("-f {arg}")]
        public int? FirstPageToConvert
        {
            get;
            set;
        }

        [ParameterSyntax("-l {arg}")]
        public int? LastPageToConvert
        {
            get;
            set;
        }

        [ParameterSyntax("-q")]
        public bool? OmitMessagesAndErrors
        {
            get;
            set;
        }

        [ParameterSyntax("-h")]
        public bool? PrintUserInformation
        {
            get;
            set;
        }

        [ParameterSyntax("-help")]
        public bool? PrintUsageInformation
        {
            get;
            set;
        }

        [ParameterSyntax("-p")]
        public bool? ExchangePDFLinksByHTML
        {
            get;
            set;
        }

        [ParameterSyntax("-c")]
        public bool? GenerateComplexDocument
        {
            get;
            set;
        }

        [ParameterSyntax("-i")]
        public bool? IgnoreImages
        {
            get;
            set;
        }

        [ParameterSyntax("-noframes")]
        public bool? GenerateNoFrames
        {
            get;
            set;
        }

        [ParameterSyntax("-stdout")]
        public bool? WriteToStdOut
        {
            get;
            set;
        }

        [ParameterSyntax("-zoom {arg}")]
        public double? Zoom
        {
            get;
            set;
        }

        [ParameterSyntax("-xml")]
        public bool? OutputForXMLPostProcessing
        {
            get;
            set;
        }

        [ParameterSyntax("-hidden")]
        public bool? OutputHiddenText
        {
            get;
            set;
        }

        [ParameterSyntax("-nomerge")]
        public bool? DoNotMergeParagraphs
        {
            get;
            set;
        }

        [ParameterSyntax("-enc {arg}")]
        public String OutputTextEncodingName
        {
            get;
            set;
        }

        [ParameterSyntax("-dev {arg}")]
        public String OutputDeviceName
        {
            get;
            set;
        }

        [ParameterSyntax("-v")]
        public bool? PrintCopyrightInfo
        {
            get;
            set;
        }

        [ParameterSyntax("-opw {arg}")]
        public String OwnerPassword
        {
            get;
            set;
        }

        [ParameterSyntax("-upw {arg}")]
        public String UserPassword
        {
            get;
            set;
        }

        [ParameterSyntax("{arg}", Position = 1, Required = true)]
        public String PDFFileLocation
        {
            get;
            set;
        }

        [ParameterSyntax("{arg}", Position = 2)]
        public String OutputFileLocation
        {
            get;
            set;
        }
    }

}
