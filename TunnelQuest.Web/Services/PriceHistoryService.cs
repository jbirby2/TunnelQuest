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
    internal class PriceHistoryService : IHostedService, IDisposable
    {
        private readonly TimeSpan sleepAfterEachItem = TimeSpan.FromSeconds(1);
        private readonly TimeSpan sleepAfterEachFullPass = TimeSpan.FromDays(1);

        private bool isRunning = false;
        private Thread workerThread;

        public PriceHistoryService()
        {
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (!isRunning)
            {
                isRunning = true;

                workerThread = new Thread(new ThreadStart(doWork));
                workerThread.Start();
            }
            

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            isRunning = false;

            if (workerThread.ThreadState == ThreadState.Suspended)
                workerThread.Abort();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            isRunning = false;
            workerThread = null;
        }

        private void doWork()
        {
            while (isRunning)
            {
                try
                {
                    using (var context = new TunnelQuestContext())
                    {
                        var serverCodes = context.Servers.Select(server => server.ServerCode).ToArray();
                        var auctionLogic = new AuctionLogic(context);

                        foreach (var serverCode in serverCodes)
                        {
                            var allItemNames = auctionLogic.GetAllItemNames(serverCode, false, false);

                            foreach (var itemName in allItemNames)
                            {
                                if (!isRunning)
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
                                    priceHistory.LifetimeMedian = itemAuctions.Median(auction => auction.Price.Value);

                                    priceHistory.UpdatedAt = DateTime.UtcNow;
                                    context.SaveChanges();
                                }
                                catch (Exception ex)
                                {
                                    // STUB log the error somewhere
                                }

                                Thread.Sleep(sleepAfterEachItem);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // STUB log the error somewhere
                }

                Thread.Sleep(sleepAfterEachFullPass);
            }
        }

    }
}
