using System;
using System.Collections.Generic;
using System.Text;

namespace TunnelQuest.AppLogic
{
    public class InvalidLogLineException : Exception
    {
        public string LogLine { get; private set; }

        public InvalidLogLineException(string logLine)
            : base("Invalid log line " + logLine)
        {
            this.LogLine = logLine;
        }
    }
}
