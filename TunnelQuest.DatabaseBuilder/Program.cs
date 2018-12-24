using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace TunnelQuest.DatabaseBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 1 && args[0].Equals("Build-Item-List", StringComparison.InvariantCultureIgnoreCase))
            {
                WikiScraper.BuildItemList(args[1]);
            }
        }
    }
}
