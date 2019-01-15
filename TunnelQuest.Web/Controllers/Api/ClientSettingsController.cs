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
    [Route("api/settings")]
    [ApiController]
    public class ClientSettingsController : ControllerBase
    {
        private TunnelQuestContext context;

        public ClientSettingsController(TunnelQuestContext _context)
        {
            this.context = _context;
        }


        // GET api/settings
        [HttpGet]
        public ClientSettings Get()
        {
            return ClientSettings.Instance;
        }

    }
}
