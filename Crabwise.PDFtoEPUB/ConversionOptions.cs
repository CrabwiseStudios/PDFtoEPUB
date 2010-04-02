using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace Crabwise.PDFtoEPUB
{
    public class Pattern : INotifyPropertyChanged
    {
        private bool _Enable;
        public bool Enable
        {
            get { return _Enable; }
            set
            {
                _Enable = value;
                Notify("Enable");
            }
        }

        private int _LineCount;
        public int LineCount
        {
            get { return _LineCount; }
            set
            {
                _LineCount = value;
                Notify("LineCount");
            }
        }

        private string _RegEx;
        public string RegEx
        {
            get { return _RegEx; }
            set
            {
                _RegEx = value;
                Notify("RegEx");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        void Notify(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
    }

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

        public string InputFile
        {
            get;
            set;
        }

        public Pattern StripHeader
        {
            get;
            set;
        }

        public Pattern StripFooter
        {
            get;
            set;
        }

        public List<string> Images
        {
            get;
            set;
        }
        public string ID
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
