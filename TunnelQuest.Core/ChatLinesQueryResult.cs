using System;
using System.Collections.Generic;
using System.Text;
using TunnelQuest.Core;
using TunnelQuest.Core.Models;

namespace TunnelQuest.Core
{
    public class ChatLinesQueryResult
    {
        public ChatLine[] ChatLines { get; set; }
        public HashSet<long> FilteredAuctionIds { get; set; }
    }
}
