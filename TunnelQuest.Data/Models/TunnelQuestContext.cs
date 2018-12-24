using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace TunnelQuest.Data.Models
{
    public class TunnelQuestContext : DbContext
    {
        public DbSet<Class> Classes { get; set; }
        public DbSet<Effect> Effects { get; set; }
        public DbSet<EffectType> EffectTypes { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemEffect> ItemEffects { get; set; }
        public DbSet<ItemStat> ItemStats { get; set; }
        public DbSet<Race> Races { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Slot> Slots { get; set; }
        public DbSet<Stat> Stats { get; set; }
        public DbSet<WeaponSkill> WeaponSkills { get; set; }


        public TunnelQuestContext ()
            : base()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            optionsBuilder.UseMySQL(configuration.GetConnectionString("TunnelQuest"));
        }
    }
}
