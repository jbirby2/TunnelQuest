using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TunnelQuest.AppLogic;
using TunnelQuest.Data;
using TunnelQuest.Data.Models;
using TunnelQuest.Web.Hubs;
using TunnelQuest.Web.Models.Api;

namespace TunnelQuest.Web.Controllers.Api
{
    [Route("api/auction_query")]
    [ApiController]
    public class AuctionQueryController : ControllerBase
    {
        private TunnelQuestContext context;

        public AuctionQueryController(TunnelQuestContext _context)
        {
            this.context = _context;
        }

        // POST api/auction_query
        [HttpPost]
        public LinesAndAuctions Get([FromBody]AuctionsQuery query)
        {
            var logic = new AuctionLogic(context);

            if (query.MaxResults == null)
                query.MaxResults = AuctionLogic.MAX_AUCTIONS;

            return new LinesAndAuctions(logic.GetAuctions(query));
        }
    }
}
