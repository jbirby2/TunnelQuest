using System;
using System.Collections.Generic;
using System.Linq;
using TunnelQuest.Data;
using TunnelQuest.Data.Models;
using System.Threading;
using TunnelQuest.AppLogic.ChatSegments;
using System.Runtime.Caching;
using Microsoft.EntityFrameworkCore;

namespace TunnelQuest.AppLogic
{
    public class ItemLogic
    {
        private TunnelQuestContext context;


        public ItemLogic(TunnelQuestContext _context)
        {
            if (_context == null)
                throw new Exception("_context cannot be null");

            this.context = _context;
        }


        public Item[] GetItems(string[] itemNames)
        {
            return context.Items
                .Where(item => itemNames.Contains(item.ItemName))
                .OrderBy(item => item.ItemName)
                .Include(item => item.Races)
                .Include(item => item.Classes)
                .Include(item => item.Deities)
                .Include(item => item.Slots)
                .Include(item => item.Info)
                .ToArray();
        }

        public PriceHistory[] GetPriceHistory(string[] itemNames)
        {
            return context.PriceHistories
                .Where(priceHistory => itemNames.Contains(priceHistory.ItemName))
                .OrderBy(priceHistory => priceHistory.ItemName)
                .ToArray();
        }
    }
}
