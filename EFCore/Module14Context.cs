using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCore
{
    public partial class Module14Context : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }


        public Module14Context()
        {
        }

        public Module14Context(DbContextOptions<Module14Context> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Module14;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

    }
}
