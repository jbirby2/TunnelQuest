using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TunnelQuest.Data.Models;
using TunnelQuest.Web.Models.Api;

namespace TunnelQuest.Web.Hubs
{
    public class AuctionHubBase : Hub
    {
        public ClientSettings GetSettings()
        {
            return new ClientSettings();
        }
    }
}
