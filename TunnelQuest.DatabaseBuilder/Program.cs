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
                var result = chatLogic.ProcessLogLine(ServerCodes.Blue, @"[Mon Dec 17 22:38:00 2018] Frinop auctions, 'WTS Tiger Skin'");
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
