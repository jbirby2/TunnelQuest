using System;
using System.Collections.Generic;
using System.Text;

namespace TunnelQuest.AppLogic
{
    internal class ChatWord
    {
        public string Text { get; private set; }

        public ChatWord (string text)
        {
            this.Text = text;
        }

        public override string ToString()
        {
            return this.Text;
        }
    }
}
