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
    internal abstract class BaseHostedService : IHostedService, IDisposable
    {
        private Thread workerThread;

        protected TimeSpan SleepAfterEachWork { get; set; }
        protected bool IsRunning { get; set; } = false;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (!IsRunning)
            {
                IsRunning = true;

                workerThread = new Thread(new ThreadStart(workerThreadStart));
                workerThread.Start();
            }
            

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            IsRunning = false;

            if (workerThread.ThreadState == ThreadState.Suspended)
                workerThread.Abort();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            IsRunning = false;
            workerThread = null;
        }

        private void workerThreadStart()
        {
            while (IsRunning)
            {
                try
                {
                    Work();
                }
                catch (Exception ex)
                {
                    // STUB log the error somewhere
                    var stub = 1;
                }

                Thread.Sleep(SleepAfterEachWork);
            }
        }

        protected abstract void Work();
    }
}
