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
                if (this.IsKnownItem)
                    return ChatLogic.ITEM_NAME_TOKEN + this.ItemName.Replace(' ', '_') + ChatLogic.ITEM_NAME_TOKEN;
                else
                    return this.ItemName;
            }
        }

        public ItemNameSegment(ParsedChatLine parentLine, string itemName, bool isKnownItem)
            : base(parentLine, itemName)
        {
            this.IsKnownItem = isKnownItem;
        }
    }
}
