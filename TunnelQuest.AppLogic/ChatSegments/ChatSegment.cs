using System;
using System.Collections.Generic;
using System.Text;

namespace TunnelQuest.AppLogic.ChatSegments
{
    internal class ChatSegment
    {
        public virtual string Text { get; protected set; }

        public ChatSegment (string text)
        {
            this.Text = text;
        }

    }
}
