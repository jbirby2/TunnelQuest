using System;
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
        public const int MIN_FILTER_ITEMNAME_LENGTH = 3;
        public const int MAX_FILTER_ITEMNAME_RESULTS = 25;


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

        // includeBuying and includeSelling only affect the query if includeUnknownItems == true
        public FilterItem[] GetFilterItems(string serverCode, string startingWith, bool includeUnknownItems, bool includeBuying, bool includeSelling)
        {
            if (includeUnknownItems == true && includeBuying == false && includeSelling == false)
                return new FilterItem[0]; // whatever.

            if (startingWith == null)
                return new FilterItem[0];

            startingWith = startingWith.Trim();

            if (startingWith.Trim().Length < MIN_FILTER_ITEMNAME_LENGTH)
                return new FilterItem[0];

            IQueryable<FilterItem> itemQuery = from item in context.Items
                                               where item.ItemName.StartsWith(startingWith)
                                               select new FilterItem() { ItemName = item.ItemName, DisplayText = item.ItemName };

            IQueryable<FilterItem> aliasQuery = from alias in context.Aliases
                                                where alias.AliasText.StartsWith(startingWith)
                                                select new FilterItem() { ItemName = alias.ItemName, DisplayText = alias.AliasText + " (aka " + alias.ItemName + ")" };

            var finalQuery = itemQuery.Union(aliasQuery);

            if (includeUnknownItems)
            {
                IQueryable<UnknownItem> unknownItemQuery = context.UnknownItems
                    .Where(unknownItem => unknownItem.ServerCode == serverCode);
                    
                if (includeBuying == false || includeSelling == false)
                    unknownItemQuery = unknownItemQuery.Where(unknownItem => unknownItem.IsBuying == includeBuying);

                // for performance it's important that this where-clause comes last, after the indexed columns
                unknownItemQuery = unknownItemQuery.Where(unknownItem => unknownItem.ItemName.StartsWith(startingWith));

                finalQuery = finalQuery.Union(unknownItemQuery.Select(unknownItem => new FilterItem(){ ItemName = unknownItem.ItemName, DisplayText = unknownItem.ItemName }));
            }

            return finalQuery
                    .OrderBy(filterItem => filterItem.DisplayText)
                    .Take(MAX_FILTER_ITEMNAME_RESULTS)
                    .ToArray();
        }
    }
}
