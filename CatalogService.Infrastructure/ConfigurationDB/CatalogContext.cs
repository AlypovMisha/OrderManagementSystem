using CatalogService.Core.Entities;
using CatalogService.Infrastructure.ConfigurationDB;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure.ConfigurationDB
{
    public class CatalogContext(DbContextOptions<CatalogContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
