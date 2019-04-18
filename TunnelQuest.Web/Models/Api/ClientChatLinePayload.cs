using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TunnelQuest.Core.Models;

namespace TunnelQuest.Web.Models.Api
{
    public class ClientChatLinePayload
    {
        public Dictionary<long, ClientChatLine> Lines { get; set; }

        public ClientChatLinePayload()
        {
        }

        public ClientChatLinePayload(IEnumerable<ChatLine> lines)
        {
            this.Lines = new Dictionary<long, ClientChatLine>();

            foreach (var line in lines)
            {
                this.Lines[line.ChatLineId] = new ClientChatLine(line);
            }
        }
    }
}