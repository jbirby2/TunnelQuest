using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace TunnelQuest.Data.Models
{
    public class TunnelQuestContext : DbContext
    {
        public DbSet<Item> Items { get; set; }

        public TunnelQuestContext (DbContextOptions<TunnelQuestContext> options)
            : base(options)
        {
        }

    }
}
