using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TunnelQuest.Web.Models.Api
{
    public class TunnelWatcherLogErrors
    {
        public string[] Errors { get; set; }

        public TunnelWatcherLogErrors()
        {
            this.Errors = new string[0];
        }

        public TunnelWatcherLogErrors(string[] _errors)
        {
            this.Errors = _errors;
        }
    }
}
