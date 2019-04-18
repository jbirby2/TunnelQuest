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
    [Route("api/chat_lines")]
    [ApiController]
    public class ChatLinesController : ControllerBase
    {
        private IHubContext<BlueChatHub> blueChatHub;
        private IHubContext<RedChatHub> redChatHub;
        private TunnelQuestContext context;

        public ChatLinesController(TunnelQuestContext _context, IHubContext<BlueChatHub> _blueChatHub, IHubContext<RedChatHub> _redChatHub)
        {
            this.context = _context;
            this.blueChatHub = _blueChatHub;
            this.redChatHub = _redChatHub;
        }

        // POST api/chat_lines
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] TunnelWatcherLog payload)
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return Unauthorized();

            string authToken;
            try
            {
                authToken = Request.Headers["Authorization"][0].Substring(7);
            }
            catch (ArgumentOutOfRangeException)
            {
                return Unauthorized();
            }

            var errors = new List<string>();
            try
            {
                var chatLogic = new ChatLogic(context);
                var addedLines = new List<ChatLine>();
                var addedAuctions = new List<Auction>();

                foreach (string line in payload.Lines)
                {
                    try
                    {
                        var newLine = chatLogic.ProcessLogLine(authToken, payload.ServerCode, line, DateTime.UtcNow);
                        addedLines.Add(newLine);
                        addedAuctions.AddRange(newLine.Auctions);
                    }
                    catch (InvalidAuthTokenException)
                    {
                        return Unauthorized();
                    }
                    catch (Exception ex)
                    {
                        errors.Add(ex.Message);
                    }
                }

                // send new chat lines to chat hub clients
                if (addedLines.Count > 0)
                {
                    var clientPayload = new ClientChatLinePayload(addedLines);
                    switch (payload.ServerCode)
                    {
                        case ServerCodes.Blue:
                            await blueChatHub.Clients.All.SendAsync("NewContent", clientPayload);
                            break;

                        case ServerCodes.Red:
                            await redChatHub.Clients.All.SendAsync("NewContent", clientPayload);
                            break;

                        default:
                            throw new Exception("Unrecognized serverCode '" + payload.ServerCode + "'");
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }

            return new JsonResult(new TunnelWatcherLogErrors(errors.ToArray()));
        }

    }
}
