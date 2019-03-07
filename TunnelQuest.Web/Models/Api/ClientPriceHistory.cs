using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TunnelQuest.Data;
using TunnelQuest.Data.Models;

namespace TunnelQuest.Web.Models.Api
{
    public class ClientPriceHistory
    {
        // static

        public static ClientPriceHistory[] Create(PriceHistory[] priceHistories)
        {
            var result = new ClientPriceHistory[priceHistories.Length];
            for (int i = 0; i < priceHistories.Length; i++)
            {
                result[i] = new ClientPriceHistory(priceHistories[i]);
            }
            return result;
        }


        // non-static

        public string ItemName { get; set; }
        public int? OneMonthMedian { get; set; }
        public int? ThreeMonthMedian { get; set; }
        public int? SixMonthMedian { get; set; }
        public int? TwelveMonthMedian { get; set; }
        public int LifetimeMedian { get; set; }
        public DateTime UpdatedAtString { get; set; } // named String even though it's a DateTime in C# because it will be serialized as a string in javascript


        public ClientPriceHistory()
        {
        }

        public ClientPriceHistory(PriceHistory priceHistory)
        {
            this.ItemName = priceHistory.ItemName;
            this.OneMonthMedian = priceHistory.OneMonthMedian;
            this.ThreeMonthMedian = priceHistory.ThreeMonthMedian;
            this.SixMonthMedian = priceHistory.SixMonthMedian;
            this.TwelveMonthMedian = priceHistory.TwelveMonthMedian;
            this.LifetimeMedian = priceHistory.LifetimeMedian;
            this.UpdatedAtString = priceHistory.UpdatedAt;
        }
    }
}
