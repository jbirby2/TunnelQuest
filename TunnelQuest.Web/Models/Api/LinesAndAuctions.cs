using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TunnelQuest.Data.Models;

namespace TunnelQuest.Web.Models.Api
{
    public class LinesAndAuctions
    {
        public ClientChatLine[] Lines { get; set; }
        public ClientAuction[] Auctions { get; set; }

        public LinesAndAuctions()
        {
        }

        public LinesAndAuctions(ChatLine[] lines)
        {
            var clientLines = new ClientChatLine[lines.Length];
            
            for (int i = 0; i < lines.Length; i++)
            {
                clientLines[i] = new ClientChatLine(lines[i]);
            }

            // Navigate backwards through the chat lines one more time to build the auctions.  This way we can easily create
            // ClientAuction objects for only the most recent version of each Auction.
            var clientAuctions = new Dictionary<long, ClientAuction>();
            for (int i = lines.Length - 1; i >= 0; i--)
            {
                foreach (var chatAuction in lines[i].Auctions)
                {
                    if (!clientAuctions.ContainsKey(chatAuction.AuctionId))
                        clientAuctions.Add(chatAuction.Auction.AuctionId, new ClientAuction(chatAuction.Auction, lines[i].ChatLineId));
                }
            }

            this.Lines = clientLines;
            this.Auctions = clientAuctions.Values.OrderBy(auction => auction.Id).ToArray();
        }
    }
}