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

        // override the default Text behavior
        public override string Text
        {
            get
            {
                return ChatLogic.OUTER_CHAT_TOKEN + "item" + ChatLogic.INNER_CHAT_TOKEN + (this.IsKnownItem ? '1' : '0') + ChatLogic.INNER_CHAT_TOKEN + this.ItemName.Replace(' ', '_') + ChatLogic.OUTER_CHAT_TOKEN;
            }
        }

        public ItemNameSegment(ParsedChatLine parentLine, string itemName, bool isKnownItem, bool hasPrecedingSpace)
            : base(parentLine, itemName, hasPrecedingSpace)
        {
            this.IsKnownItem = isKnownItem;
        }
    }
}
