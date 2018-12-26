using System;
using System.Collections.Generic;
using System.Text;

namespace TunnelQuest.Data.Migrations.Data
{
    public class UnknownStatTokenException : Exception
    {
        public UnknownStatTokenException(string token)
            : base("Unknown token while parsing effectTypeCode \"" + token + "\"")
        {
        }
    }
}
