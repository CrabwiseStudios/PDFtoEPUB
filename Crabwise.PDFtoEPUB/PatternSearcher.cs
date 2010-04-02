using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Crabwise.PDFtoEPUB
{
    internal struct PatternFound
    {
        public string TextBeforePattern;
        public string Pattern;
        public bool EndOfStreamReached;

        public PatternFound(string textBeforePattern, string pattern, bool endOfStreamReached)
        {
            TextBeforePattern = textBeforePattern;
            Pattern = pattern;
            EndOfStreamReached = endOfStreamReached;
        }
    }

    internal class PatternSearcher
    {
        private StreamReader _Stream;
        private string _TextBuffer;
        private int _TextOffset;
        private int _BufferSize = 8192;

        public PatternSearcher(StreamReader reader, int bufferSize)
        {
            _Stream = reader;
            _TextBuffer = "";
            _TextOffset = 0;
            _BufferSize = bufferSize;
        }

        public PatternFound NextPattern(string pattern)
        {
            if (string.IsNullOrEmpty(_TextBuffer))
            {
                FillBuffer();
            }

            Regex regExpression = CreateSearchExpression(pattern);
            return FindNextPattern(regExpression);
        }


        private void FillBuffer()
        {
            char[] buffer = new char[_BufferSize];
            int size = _Stream.ReadBlock(buffer, 0, _BufferSize);

            _TextBuffer = _TextBuffer.Substring(_TextOffset) + new string(buffer, 0, size);
            _TextOffset = 0;
        }

        private Regex CreateSearchExpression(string pattern)
        {
            Regex regex = new Regex(pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            return regex;
        }

        private PatternFound FindNextPattern(Regex regex)
        {
            Match match = regex.Match(_TextBuffer, _TextOffset);
            if (match.Success == true)
            {
                int orgOffset = _TextOffset;
                _TextOffset = match.Index + match.Length;
                return new PatternFound(_TextBuffer.Substring(orgOffset, match.Index - orgOffset), _TextBuffer.Substring(match.Index, match.Length), false);
            }

            if (_Stream.EndOfStream == true)
            {
                return new PatternFound(_TextBuffer.Substring(_TextOffset), null, true);
            }
            else
            {
                FillBuffer();
                return FindNextPattern(regex);
            }
        }

    }
}
