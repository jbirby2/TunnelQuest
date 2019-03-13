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
    public class HomeController : ControllerBase
    {
        public HomeController()
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            return File("~/index.html", "text/html");
        }

    }
}
