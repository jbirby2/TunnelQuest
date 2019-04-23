using System;
using System.Collections.Generic;
using System.Text;
using TunnelQuest.Core;

namespace TunnelQuest.Core
{
    public class ChatLinesQuery
    {
        public string ServerCode { get; set; } = ServerCodes.Blue;
        public long? MinimumId { get; set; } = null;
        public long? MaximumId { get; set; } = null;
        public int? MaxResults { get; set; } = null;
        public ChatFilterSettings FilterSettings { get; set; } = null;
    }
}
