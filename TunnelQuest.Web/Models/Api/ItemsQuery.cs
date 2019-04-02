using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TunnelQuest.Web.Models.Api
{
    public class ItemsQuery
    {
        public string ServerCode { get; set; }
        public string[] ItemNames { get; set; } = new string[0];
    }
}
