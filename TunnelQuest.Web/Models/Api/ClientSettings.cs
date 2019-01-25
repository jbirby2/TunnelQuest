using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TunnelQuest.AppLogic;
using TunnelQuest.Data.Models;

namespace TunnelQuest.Web.Models.Api
{
    public class ClientSettings
    {
        // static stuff

        public static readonly ClientSettings Instance = new ClientSettings();


        // non-static stuff

        public string AuctionToken { get; set; }
        public int MaxChatLines { get; set; }
        public int MaxAuctions { get; set; }

        private ClientSettings()
        {
            this.AuctionToken = ChatLogic.AUCTION_TOKEN;
            this.MaxChatLines = ChatLogic.MAX_CHAT_LINES;
            this.MaxAuctions = AuctionLogic.MAX_AUCTIONS;
        }

    }
}
