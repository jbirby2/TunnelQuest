using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TunnelQuest.Data.Models;

namespace TunnelQuest.Web.Models.Api
{
    public class ClientChatLineToken
    {
        public string Type { get; set; }
        public Dictionary<string, string> Properties { get; set; }

        public ClientChatLineToken()
        {
        }

        public ClientChatLineToken(ChatLineToken token)
        {
            this.Type = token.TokenTypeCode;

            this.Properties = new Dictionary<string, string>();
            foreach (var tokenProp in token.Properties)
            {
                this.Properties.Add(tokenProp.Property, tokenProp.Value);
            }
        }

    }
}
