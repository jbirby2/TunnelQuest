using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using TunnelQuest.Core;
using TunnelQuest.Core.Migrations;
using TunnelQuest.Core.Models;

namespace TunnelQuest.DatabaseBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            //stub
            new InsertChatLogData().STUB();
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
