using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Crabwise.PDFtoEPUB
{
    public class ConversionOptions
    {
        public Image CoverImage
        {
            get;
            set;
        }
        public string Title
        {
            get;
            set;
        }
        public string Author
        {
            get;
            set;
        }
        public DateTime PublishDate
        {
            get;
            set;
        }
        public List<Chapter> Chapters
        {
            get;
            set;
        }
        public bool IncludeImages
        {
            get;
            set;
        }

    }
    public class Chapter : IComparable
    {
        public string ChapterTitle
        {
            get;
            set;
        }
        public string EmbeddedTitle
        {
            get;
            set;
        }
        public int ChapterNumber
        {
            get;
            set;
        }


        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj is Chapter)
            {
                return ChapterNumber - ((Chapter)obj).ChapterNumber;
            }
            else
                throw new ArgumentException();
        }

        #endregion
    }
}
