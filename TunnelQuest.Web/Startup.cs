using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.HttpOverrides;
using TunnelQuest.Core.Models;
using TunnelQuest.Web.Config;
using TunnelQuest.Web.Hubs;
using Microsoft.Extensions.Hosting;
using TunnelQuest.Web.Services;

namespace TunnelQuest.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var settings = new TunnelQuestSettings();
            Configuration.Bind("TunnelQuest", settings);
            services.AddSingleton(settings);

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                 .AddJsonOptions(options => {
                     options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                 });

            services.AddCors();
            services.AddDbContext<TunnelQuestContext>(/*options => 
                options.UseMySQL(Configuration.GetConnectionString("TunnelQuest"))
            */);

            // STUB add api throttling

            services.AddSignalR();

            // custom TunnelQuest services
            services.AddHostedService<PriceHistoryService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions()
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            
            // remember: the order of the blocks below is very important

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseSignalR(route =>
            {
                route.MapHub<BlueAuctionHub>("/blue_auction_hub");
                route.MapHub<BlueChatHub>("/blue_chat_hub");
                route.MapHub<RedChatHub>("/red_chat_hub");
                route.MapHub<RedAuctionHub>("/red_auction_hub");
            });

            app.UseMvc(configureRoutes => 
            {
                configureRoutes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");

                configureRoutes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });

            
        }
    }
}
