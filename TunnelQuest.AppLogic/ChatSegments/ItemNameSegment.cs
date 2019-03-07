using System;
using System.Collections.Generic;
using System.Text;
using TunnelQuest.Data.Models;

namespace TunnelQuest.AppLogic.ChatSegments
{
    internal class ItemNameSegment : TextSegment
    {
        public string ItemName
        {
            get { return base.Text; }
        }

        public bool IsKnownItem { get; private set; }

        public ItemNameSegment(ParsedChatLine parentLine, string itemName, bool isKnownItem, bool hasPrecedingSpace)
            : base(parentLine, itemName, hasPrecedingSpace)
        {
            this.IsKnownItem = isKnownItem;
        }
    }
}
