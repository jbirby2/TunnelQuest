using System;
using System.Collections.Generic;
using System.Text;
using TunnelQuest.Data;

namespace TunnelQuest.AppLogic
{
    public class AuctionsQuery
    {
        public string ServerCode { get; set; } = ServerCodes.Blue;
        public string ItemName { get; set; } = null;
        public bool IncludeChatLine { get; set; } = false;
        public bool IncludeBuying { get; set; } = true;
        public bool IncludeUnpriced { get; set; } = true;
        public long? MinimumId { get; set; } = null;
        public long? MaximumId { get; set; } = null;
        public int? MaxResults { get; set; } = null;
    }
}
