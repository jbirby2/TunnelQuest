using System;
using System.Collections.Generic;
using System.Text;
using TunnelQuest.Core.Models;

namespace TunnelQuest.Core.ChatSegments
{
    internal class ItemNameSegment : TextSegment
    {
        public string ItemName { get; private set; }
        public string AliasText { get; private set; }
        public bool IsKnownItem { get; private set; }
        public Auction Auction { get; set; }

        public ItemNameSegment(string text, string itemName, bool isKnownItem, bool hasPrecedingSpace)
            : base(text, hasPrecedingSpace)
        {
            this.ItemName = itemName;
            this.IsKnownItem = isKnownItem;

            if (text != itemName)
                this.AliasText = text;
            else
                this.AliasText = null;
        }

        public override string Text
        {
            get
            {
                return ChatLogic.CHAT_TOKEN + this.Auction.AuctionId.ToString() + ChatLogic.CHAT_TOKEN;
            }
        }
    }
}
