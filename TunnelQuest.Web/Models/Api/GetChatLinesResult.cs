using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TunnelQuest.Data.Models;

namespace TunnelQuest.Web.Models.Api
{
    public class GetChatLinesResult
    {
        public IEnumerable<ClientChatLine> Lines { get; set; }
        public IEnumerable<ClientAuction> Auctions { get; set; }

        public GetChatLinesResult()
        {
        }

        public GetChatLinesResult(ChatLine[] lines)
        {
            var clientChatLines = new List<ClientChatLine>();
            foreach (var chatLine in lines)
            {
                clientChatLines.Add(new ClientChatLine(chatLine));
            }

            // Navigate backwards through the chat lines one more time to build the auctions.  This way we can easily create
            // ClientAuction objects for only the most recent version of each Auction.
            var clientAuctions = new Dictionary<long, ClientAuction>();
            for (int i = lines.Length - 1; i >= 0; i--)
            {
                foreach (var chatAuction in lines[i].Auctions)
                {
                    if (!clientAuctions.ContainsKey(chatAuction.AuctionId))
                        clientAuctions.Add(chatAuction.Auction.AuctionId, new ClientAuction(chatAuction.Auction));
                }
            }

            this.Lines = clientChatLines;
            this.Auctions = clientAuctions.Values.OrderBy(auction => auction.Id).ToArray(); // I *think* they would naturally be ordered by their Id anyway, but just in case...
        }
    }
}
