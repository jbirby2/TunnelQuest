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
            /*
            using (var context = new TunnelQuestContext())
            {
                string authToken = "7384321a-473d-4f09-81e6-99f00802425d";

                var chatLogic = new ChatLogic(context);
                chatLogic.ProcessLogLine(authToken, ServerCodes.Blue, @"[Sun Dec 16 22:11:05 2018] Madworld auctions, 'WTS/T Manna Robe 140k  Tanglewood Shield 35k  Circlet of Shadow (pre-nerf) 25k  Matchless Dragonhorn Bracers 10k/ea  Silver Chitin Wristband 4.5k  Netted Kelp Tunic 2k'");
                chatLogic.ProcessLogLine(authToken, ServerCodes.Blue, @"[Sun Dec 16 22:11:05 2018] Madworld auctions, 'WTS/T Manna Robe 140k  Tanglewood Shield 35k  Circlet of Shadow (pre-nerf) 25k  Matchless Dragonhorn Bracers 10k/ea  Silver Chitin Wristband 4.5k  Netted Kelp Tunic 2k'");
                chatLogic.ProcessLogLine(authToken, ServerCodes.Blue, @"[Sun Dec 16 22:11:05 2018] Madworld auctions, 'WTS/T Manna Robe 140k  Tanglewood Shield 35k  Circlet of Shadow (pre-nerf) 25k  Matchless Dragonhorn Bracers 10k/ea  Silver Chitin Wristband 4.5k  Netted Kelp Tunic 2k'");
                var stub = 1;
            }
            return;
            */

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
