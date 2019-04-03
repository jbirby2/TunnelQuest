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
    [Route("api/filter_items")]
    [ApiController]
    public class FilterItemsController : ControllerBase
    {
        private TunnelQuestContext context;

        public FilterItemsController(TunnelQuestContext _context)
        {
            this.context = _context;
        }

        [HttpGet]
        public FilterItem[] Get(string serverCode, string startingWith, bool includeUnknownItems, bool includeBuying, bool includeSelling)
        {
            var itemLogic = new ItemLogic(context);
            return itemLogic.GetFilterItems(serverCode, startingWith, includeUnknownItems, includeBuying, includeSelling);
        }

    }
}
