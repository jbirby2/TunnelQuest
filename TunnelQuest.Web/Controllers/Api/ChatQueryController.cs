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
    [Route("api/chat_query")]
    [ApiController]
    public class ChatQueryController : ControllerBase
    {
        private TunnelQuestContext context;

        public ChatQueryController(TunnelQuestContext _context)
        {
            this.context = _context;
        }

        // POST api/chat_query
        [HttpPost]
        public LinesAndAuctions Get([FromBody]ChatLinesQuery query)
        {
            if (query.MaxResults == null)
                query.MaxResults = ChatLogic.MAX_CHAT_LINES;

            var chatLines = new ChatLogic(context).GetLines(query);
            return new LinesAndAuctions(chatLines);
        }

    }
}
