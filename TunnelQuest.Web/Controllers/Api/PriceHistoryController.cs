using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TunnelQuest.Core;
using TunnelQuest.Core.Models;
using TunnelQuest.Web.Hubs;
using TunnelQuest.Web.Models.Api;

namespace TunnelQuest.Web.Controllers.Api
{
    [Route("api/price_history")]
    [ApiController]
    public class PriceHistoryController : ControllerBase
    {
        private TunnelQuestContext context;

        public PriceHistoryController(TunnelQuestContext _context)
        {
            this.context = _context;
        }

        [HttpPost]
        public ClientPriceHistory[] Post([FromBody]ItemsQuery query)
        {
            var itemLogic = new ItemLogic(context);
            return ClientPriceHistory.Create(itemLogic.GetPriceHistory(query.ItemNames));
        }
    }
}
