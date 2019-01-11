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
        public string AuctionToken { get; set; }
        
        public ClientSettings()
        {
            this.AuctionToken = ChatLogic.AUCTION_TOKEN;
        }

    }
}
