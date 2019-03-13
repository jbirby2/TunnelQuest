using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TunnelQuest.Core.Models;

namespace TunnelQuest.Web.Models.Api
{
    public class LinesAndAuctions
    {
        public Dictionary<long, ClientChatLine> Lines { get; set; }
        public Dictionary<long, ClientAuction> Auctions { get; set; }

        public LinesAndAuctions()
        {
        }

        public LinesAndAuctions(ChatLine[] lines)
        {
            this.Lines = new Dictionary<long, ClientChatLine>();
            this.Auctions = new Dictionary<long, ClientAuction>();

            foreach (var line in lines)
            {
                this.Lines[line.ChatLineId] = new ClientChatLine(line);
            }
        }

        public LinesAndAuctions(Auction[] auctions)
        {
            this.Lines = new Dictionary<long, ClientChatLine>();
            this.Auctions = new Dictionary<long, ClientAuction>();

            foreach (var auction in auctions)
            {
                this.Auctions[auction.AuctionId] = new ClientAuction(auction);
                if (auction.MostRecentChatLine != null)
                    this.Lines[auction.MostRecentChatLine.ChatLineId] = new ClientChatLine(auction.MostRecentChatLine);
            }
        }
    }
}