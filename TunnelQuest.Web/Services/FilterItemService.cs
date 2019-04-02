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
    internal class FilterItemService : BaseHostedService
    {
        private readonly TimeSpan sleepAfterEachItem = TimeSpan.FromSeconds(1);

        public FilterItemService()
        {
            this.SleepAfterEachWork = TimeSpan.FromMinutes(20);
        }

        protected override void Work()
        {
            var context = new TunnelQuestContext();
            var serverCodes = context.Servers.Select(server => server.ServerCode).ToArray();
            int contextResetCounter = 0;

            foreach (var serverCode in serverCodes)
            {
                /*
                itemNames = (from item in context.Items
                             orderby item.ItemName
                             select item.ItemName).ToArray();

                itemAliases = (from alias in context.Aliases
                               orderby alias.AliasText
                               select alias).ToArray();
                */

                /*
                var allItemNames = (from auction in context.Auctions
                                    where
                                       auction.ServerCode == serverCode
                                       && auction.IsBuying == false
                                       && auction.Price != null
                                       && auction.Price > 0
                                    select auction.ItemName).Distinct().OrderBy(itemName => itemName).ToArray();


                // create a new context every so often to work around an apparent bug in the Pomelo MySQL data provider
                // that causes it to gradually execute queries slower and slower the more it's used
                contextResetCounter++;
                if (contextResetCounter > 100)
                {
                    contextResetCounter = 0;
                    context.Dispose();
                    context = new TunnelQuestContext();
                }
                */
            }
        }

    }
}
