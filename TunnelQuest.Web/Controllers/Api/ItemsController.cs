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
    [Route("api/items")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private TunnelQuestContext context;

        public ItemsController(TunnelQuestContext _context)
        {
            this.context = _context;
        }

        [HttpPost]
        public ClientItem[] Post([FromBody]ItemsQuery query)
        {
            var itemLogic = new ItemLogic(context);
            return ClientItem.Create(itemLogic.GetItems(query.ItemNames));
        }

    }
}
