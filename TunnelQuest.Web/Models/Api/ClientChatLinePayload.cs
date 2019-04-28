using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TunnelQuest.Core;
using TunnelQuest.Core.Models;

namespace TunnelQuest.Web.Models.Api
{
    public class ClientChatLinePayload
    {
        public ClientChatLine[] Lines { get; set; }

        public ClientChatLinePayload()
        {
        }

        // used for payloads sent through the live connection hub
        public ClientChatLinePayload(List<ChatLine> chatLines)
        {
            this.Lines = new ClientChatLine[chatLines.Count];

            int i = 0;
            foreach (var line in chatLines)
            {
                this.Lines[i] = new ClientChatLine(line, null);
                i++;
            }
        }

        // used for payloads returned by API queries
        public ClientChatLinePayload(ChatLinesQueryResult queryResult)
        {
            this.Lines = new ClientChatLine[queryResult.ChatLines.Length];

            int i = 0;
            foreach (var line in queryResult.ChatLines)
            {
                this.Lines[i] = new ClientChatLine(line, queryResult.FilteredAuctionIds);
                i++;
            }
        }
    }
}