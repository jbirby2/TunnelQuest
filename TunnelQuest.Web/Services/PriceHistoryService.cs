using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TunnelQuest.AppLogic;
using TunnelQuest.Data.Models;

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
                using (var context = new TunnelQuestContext())
                {
                    var serverCodes = context.Servers.Select(server => server.ServerCode).ToArray();
                    var auctionLogic = new AuctionLogic(context);

                    foreach (var serverCode in serverCodes)
                    {
                        var allItemNames = auctionLogic.GetAllItemNames(serverCode);

                        foreach (var itemName in allItemNames)
                        {
                            if (!isRunning)
                                return;

                            var itemAuctions = auctionLogic.GetAuctions(serverCode, false, itemName);

                            // STUB left off here to go implement a better way to pull most recent chat line in AuctionLogic.GetAuctions()

                            Thread.Sleep(sleepAfterEachItem);
                        }
                    }
                }

                Thread.Sleep(sleepAfterEachFullPass);
            }
        }
    }
}
