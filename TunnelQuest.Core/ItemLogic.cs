﻿using System;
using System.Collections.Generic;
using System.Linq;
using TunnelQuest.Core;
using TunnelQuest.Core.Models;
using System.Threading;
using TunnelQuest.Core.ChatSegments;
using System.Runtime.Caching;
using Microsoft.EntityFrameworkCore;

namespace TunnelQuest.Core
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

        public PriceHistory[] GetPriceHistory(string serverCode, string[] itemNames)
        {
            return context.PriceHistories
                .Where(priceHistory => priceHistory.ServerCode == serverCode && itemNames.Contains(priceHistory.ItemName))
                .OrderBy(priceHistory => priceHistory.ItemName)
                .ToArray();
        }

        public Alias[] GetAliases()
        {
            return context.Aliases
                .OrderBy(alias => alias.AliasText)
                .ToArray();
        }
    }
}
