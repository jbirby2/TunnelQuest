using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TunnelQuest.Core;
using TunnelQuest.Core.Models;

namespace TunnelQuest.Web.Services
{
    internal class PriceHistoryService : BaseHostedService
    {
        private readonly TimeSpan sleepAfterEachItem = TimeSpan.FromSeconds(1);

        public PriceHistoryService()
        {
            this.SleepAfterEachWork = TimeSpan.FromDays(1);
        }

        protected override void Work()
        {
            var context = new TunnelQuestContext();
            var serverCodes = context.Servers.Select(server => server.ServerCode).ToArray();
            var auctionLogic = new AuctionLogic(context);
            int contextResetCounter = 0;

            foreach (var serverCode in serverCodes)
            {
                var allItemNames = (from auction in context.Auctions
                                    where
                                       auction.ServerCode == serverCode
                                       && auction.IsBuying == false
                                       && auction.Price != null
                                       && auction.Price > 0
                                    select auction.ItemName).Distinct().OrderBy(itemName => itemName).ToArray();

                foreach (var itemName in allItemNames)
                {
                    if (!IsRunning)
                        return;

                    try
                    {
                        Auction[] itemAuctions = auctionLogic.GetAuctions(new AuctionsQuery()
                        {
                            ServerCode = serverCode,
                            ItemName = itemName,
                            IncludeChatLine = false,
                            IncludeBuying = false,
                            IncludeUnpriced = false,
                            MaxResults = null
                        });

                        DateTime? oldestAuctionDate = itemAuctions.Max(auction => auction.CreatedAt);

                        var priceHistory = context.PriceHistories.Where(pHistory => pHistory.ItemName.Equals(itemName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                        if (priceHistory == null)
                        {
                            priceHistory = new PriceHistory();
                            priceHistory.ServerCode = serverCode;
                            priceHistory.ItemName = itemName;
                            priceHistory.CreatedAt = DateTime.UtcNow;
                            context.PriceHistories.Add(priceHistory);
                        }

                        // 1 month median
                        DateTime oneMonthAgo = DateTime.UtcNow.AddMonths(-1);
                        if (oldestAuctionDate <= oneMonthAgo)
                            priceHistory.OneMonthMedian = itemAuctions.Where(auction => auction.UpdatedAt >= oneMonthAgo).Median(auction => auction.Price.Value);
                        else
                            priceHistory.OneMonthMedian = null;

                        // 3 month median
                        DateTime threeMonthsAgo = DateTime.UtcNow.AddMonths(-3);
                        if (oldestAuctionDate <= threeMonthsAgo)
                            priceHistory.ThreeMonthMedian = itemAuctions.Where(auction => auction.UpdatedAt >= threeMonthsAgo).Median(auction => auction.Price.Value);
                        else
                            priceHistory.ThreeMonthMedian = null;

                        // 6 month median
                        DateTime sixMonthsAgo = DateTime.UtcNow.AddMonths(-6);
                        if (oldestAuctionDate <= sixMonthsAgo)
                            priceHistory.SixMonthMedian = itemAuctions.Where(auction => auction.UpdatedAt >= sixMonthsAgo).Median(auction => auction.Price.Value);
                        else
                            priceHistory.SixMonthMedian = null;

                        // 12 month median
                        DateTime twelveMonthsAgo = DateTime.UtcNow.AddMonths(-12);
                        if (oldestAuctionDate <= twelveMonthsAgo)
                            priceHistory.TwelveMonthMedian = itemAuctions.Where(auction => auction.UpdatedAt >= twelveMonthsAgo).Median(auction => auction.Price.Value);
                        else
                            priceHistory.TwelveMonthMedian = null;

                        // lifetime median
                        priceHistory.LifetimeMedian = itemAuctions.Median(auction => auction.Price.Value).Value;

                        priceHistory.UpdatedAt = DateTime.UtcNow;
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        // STUB log the error somewhere
                        var stub = 1;
                    }

                    // create a new context every so often to work around an apparent bug in the Pomelo MySQL data provider
                    // that causes it to gradually execute queries slower and slower the more it's used
                    contextResetCounter++;
                    if (contextResetCounter > 100)
                    {
                        contextResetCounter = 0;
                        context.Dispose();
                        context = new TunnelQuestContext();
                        auctionLogic = new AuctionLogic(context);
                    }

                    if (!System.Diagnostics.Debugger.IsAttached)
                        Thread.Sleep(sleepAfterEachItem);
                } // end foreach(itemName)
            }
        }

    }
}
