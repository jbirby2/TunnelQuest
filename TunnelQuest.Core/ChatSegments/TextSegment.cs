using System;
using System.Collections.Generic;
using System.Text;

namespace TunnelQuest.Core.ChatSegments
{
    internal class TextSegment
    {
        // static helper methods

        protected static TextSegment GetNextSegment(List<TextSegment> segments, int index)
        {
            int nextIndex = index + 1;
            if (nextIndex < segments.Count)
                return segments[nextIndex];
            else
                return null;
        }

        protected static TextSegment GetPrevSegment(List<TextSegment> segments, int index)
        {
            int prevIndex = index - 1;
            if (prevIndex >= 0)
                return segments[prevIndex];
            else
                return null;
        }

        // non-static stuff

        public bool HasPrecedingSpace { get; private set; }
        public virtual string Text { get; private set; }

        public TextSegment (string text, bool hasPrecedingSpace)
        {
            this.HasPrecedingSpace = hasPrecedingSpace;
            this.Text = text ?? "";
        }

    }
}
