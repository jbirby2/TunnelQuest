using System;
using System.Collections.Generic;
using System.Text;

namespace TunnelQuest.Core.Migrations.Data
{
    public class UnknownStatTokenException : Exception
    {
        public UnknownStatTokenException(string token)
            : base("Unknown token while parsing effectTypeCode \"" + token + "\"")
        {
        }
    }
}
