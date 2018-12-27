using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using TunnelQuest.Data.Migrations;

namespace TunnelQuest.DatabaseBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 4 && args[0].Equals("Build-Item-Details", StringComparison.InvariantCultureIgnoreCase))
                    WikiScraper.BuildItemDetails(args[1], args[2], Convert.ToBoolean(args[3]));
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
