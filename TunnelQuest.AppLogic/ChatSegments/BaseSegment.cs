using System;
using System.Collections.Generic;
using System.Text;

namespace TunnelQuest.AppLogic.ChatSegments
{
    internal class BaseSegment
    {
        public ParsedChatLine ParentLine { get; private set; }
        public virtual string Text { get; protected set; }

        public BaseSegment (ParsedChatLine parentLine, string text)
        {
            if (parentLine == null)
                throw new Exception("parentLine cannot be null");
            
            this.ParentLine = parentLine;
            this.Text = text;
        }

        public BaseSegment NextSegment()
        {
            return ParentLine.NextSegment(this);
        }

        public BaseSegment PrevSegment()
        {
            return ParentLine.PrevSegment(this);
        }
    }
}
