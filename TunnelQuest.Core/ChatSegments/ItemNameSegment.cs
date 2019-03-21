using System;
using System.Collections.Generic;
using System.Text;
using TunnelQuest.Core.Models;

namespace TunnelQuest.Core.ChatSegments
{
    internal class ItemNameSegment : TextSegment
    {
        public string ItemName { get; private set; }
        public bool IsKnownItem { get; private set; }

        public ItemNameSegment(ParsedChatLine parentLine, string text, string itemName, bool isKnownItem, bool hasPrecedingSpace)
            : base(parentLine, text, hasPrecedingSpace)
        {
            this.ItemName = itemName;
            this.IsKnownItem = isKnownItem;
        }
    }
}
