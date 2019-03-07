using System;
using System.Collections.Generic;
using System.Text;

namespace TunnelQuest.AppLogic.ChatSegments
{
    internal class TextSegment
    {
        public bool IsTokenized { get; set; }
        public bool HasPrecedingSpace { get; private set; }
        public ParsedChatLine ParentLine { get; private set; }
        public string Text { get; protected set; }

        public TextSegment (ParsedChatLine parentLine, string text, bool hasPrecedingSpace)
        {
            if (parentLine == null)
                throw new Exception("parentLine cannot be null");

            this.HasPrecedingSpace = hasPrecedingSpace;
            this.ParentLine = parentLine;
            this.Text = text ?? "";
        }

        public TextSegment NextSegment()
        {
            return ParentLine.NextSegment(this);
        }

        public TextSegment PrevSegment()
        {
            return ParentLine.PrevSegment(this);
        }
    }
}
