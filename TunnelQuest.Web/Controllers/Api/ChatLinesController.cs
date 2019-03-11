﻿using System;
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
    [Route("api/chat_lines")]
    [ApiController]
    public class ChatLinesController : ControllerBase
    {
        private IHubContext<BlueChatHub> blueChatHub;
        private IHubContext<BlueAuctionHub> blueAuctionHub;
        private IHubContext<RedChatHub> redChatHub;
        private IHubContext<RedAuctionHub> redAuctionHub;
        private TunnelQuestContext context;

        public ChatLinesController(TunnelQuestContext _context, 
            IHubContext<BlueChatHub> _blueChatHub,
            IHubContext<BlueAuctionHub> _blueAuctionHub,
            IHubContext<RedChatHub> _redChatHub,
            IHubContext<RedAuctionHub> _redAuctionHub)
        {
            this.context = _context;
            this.blueChatHub = _blueChatHub;
            this.blueAuctionHub = _blueAuctionHub;
            this.redChatHub = _redChatHub;
            this.redAuctionHub = _redAuctionHub;
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
                        var results = chatLogic.ProcessLogLine(authToken, payload.ServerCode, line);

                        if (results != null)
                        {
                            addedLines.Add(results.NewLine);
                            addedAuctions.AddRange(results.NewAuctions);
                        }
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
                    var newChatContent = new LinesAndAuctions(addedLines.ToArray());
                    switch (payload.ServerCode)
                    {
                        case ServerCodes.Blue:
                            await blueChatHub.Clients.All.SendAsync("NewContent", newChatContent);
                            break;

                        case ServerCodes.Red:
                            await redChatHub.Clients.All.SendAsync("NewContent", newChatContent);
                            break;

                        default:
                            throw new Exception("Unrecognized serverCode '" + payload.ServerCode + "'");
                    }
                }

                // send new auctions to auction hub clients
                if (addedAuctions.Count > 0)
                {
                    var newAuctionContent = new LinesAndAuctions(addedAuctions.ToArray());
                    switch (payload.ServerCode)
                    {
                        case ServerCodes.Blue:
                            await blueAuctionHub.Clients.All.SendAsync("NewContent", newAuctionContent);
                            break;

                        case ServerCodes.Red:
                            await redAuctionHub.Clients.All.SendAsync("NewContent", newAuctionContent);
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
