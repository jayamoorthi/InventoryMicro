using Microsoft.EntityFrameworkCore;
using ProductApi.Domain.DomainModels;
using ProductApi.Infrastructure.EntityConfigurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Infrastructure
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) :base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SellingEntityConfiguration());
            modelBuilder.ApplyConfiguration(new InventoryStockEntityConfiguration());
        }

        public DbSet<InventoryStock> InventoryStock { get; set; }

        public DbSet<Selling> Selling { get; set; }
    }
}
