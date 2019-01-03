using System;
using System.Collections.Generic;
using System.Text;

namespace TunnelQuest.AppLogic
{
    internal class ItemNameWord : ChatWord
    {
        public int ItemNameIndex { get; private set; }

        public ItemNameWord (string text, int itemNameIndex)
            : base(text)
        {
            this.ItemNameIndex = itemNameIndex;
        }
    }
}
