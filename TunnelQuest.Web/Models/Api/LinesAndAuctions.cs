using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TunnelQuest.Data.Models;

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
                foreach (var lineAuction in line.Auctions)
                {
                    this.Auctions[lineAuction.AuctionId] = new ClientAuction(lineAuction.Auction, line.ChatLineId);
                }
            }
        }

        public LinesAndAuctions(Auction[] auctions)
        {
            this.Lines = new Dictionary<long, ClientChatLine>();
            this.Auctions = new Dictionary<long, ClientAuction>();

            foreach (var auction in auctions)
            {
                this.Auctions[auction.AuctionId] = new ClientAuction(auction, auction.ChatLines.First().ChatLineId);
                foreach (var auctionLine in auction.ChatLines)
                {
                    this.Lines[auctionLine.ChatLineId] = new ClientChatLine(auctionLine.ChatLine);
                }
            }
        }
    }
}