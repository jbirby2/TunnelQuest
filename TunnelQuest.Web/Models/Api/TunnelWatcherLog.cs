using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TunnelQuest.Web.Models.Api
{
    public class TunnelWatcherLog
    {
        public string ServerCode { get; set; }
        public string[] Lines { get; set; } = new string[0];
    }
}
