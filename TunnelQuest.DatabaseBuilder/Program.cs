using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using TunnelQuest.AppLogic;
using TunnelQuest.Data;
using TunnelQuest.Data.Migrations;
using TunnelQuest.Data.Models;

namespace TunnelQuest.DatabaseBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            //stub
            using (var context = new TunnelQuestContext())
            {
                var chatLogic = new ChatLogic(context);
                var result = chatLogic.ProcessLogLine(ServerCodes.Blue, @"[Sun Dec 16 22:09:23 2018] Luluo auctions, 'WTS Velium Fire Wedding Ring 500 Spell: Celestial Healing 500 stub Velium Fire Wedding Ring stub Velium Fire Wedding Ring stub Nephrite 100 Kobold Jester's Crown 1000 Crystal Chitin Gauntlets 600 Golden Efreeti Boots 1100 Velium Fire Wedding Ring'");
                var stub = 1;
            }
            return;

            try
            {
                if (args.Length == 4 && args[0].Equals("Scrape", StringComparison.InvariantCultureIgnoreCase))
                    WikiScraper.Scrape(args[1], args[2], Convert.ToBoolean(args[3]));
                else if (args.Length == 4 && args[0].Equals("Scrape-Spells", StringComparison.InvariantCultureIgnoreCase))
                    WikiScraper.ScrapeSpells(args[1], args[2], Convert.ToBoolean(args[3]));
                else if (args.Length == 4 && args[0].Equals("Scrape-Items", StringComparison.InvariantCultureIgnoreCase))
                    WikiScraper.ScrapeItems(args[1], args[2], Convert.ToBoolean(args[3]));
                else if (args.Length == 2 && args[0].Equals("List-Duplicate-Names", StringComparison.InvariantCultureIgnoreCase))
                    WikiScraper.ListDuplicateNames(args[1]);
                else
                    Console.WriteLine("Invalid command arguments.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unhandled Exception:" + Environment.NewLine + ex.ToString());
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
