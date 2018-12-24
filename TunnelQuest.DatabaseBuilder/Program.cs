using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TunnelQuest.DatabaseBuilder
{
    // Add-Migration -Name ___________________ -Project TunnelQuest.Data -StartupProject TunnelQuest.DatabaseBuilder

    // Update-Database -Project TunnelQuest.Data -StartupProject TunnelQuest.DatabaseBuilder



    // Remove-Migration -Project TunnelQuest.Data -StartupProject TunnelQuest.DatabaseBuilder


    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
