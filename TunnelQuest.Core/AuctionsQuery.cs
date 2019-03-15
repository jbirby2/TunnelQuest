using System;
using System.Collections.Generic;
using System.Text;
using TunnelQuest.Core;

namespace TunnelQuest.Core
{
    public class AuctionsQuery
    {
        public string ServerCode { get; set; } = ServerCodes.Blue;
        public string ItemName { get; set; } = null;
        public bool IncludeChatLine { get; set; } = false;
        public bool IncludeBuying { get; set; } = true;
        public bool IncludeUnpriced { get; set; } = true;
        public DateTime? MinimumUpdatedAt { get; set; } = null;
        public DateTime? MaximumUpdatedAt { get; set; } = null;
        public int? MaxResults { get; set; } = null;
    }
}
