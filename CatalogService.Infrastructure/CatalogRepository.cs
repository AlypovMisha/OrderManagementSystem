using CatalogService.Core.Entities;
using CatalogService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure
{
    public class CatalogRepository(CatalogDbContext catalogDbContext)
    {
        public async Task CreateProductAsync()
        {
            await catalogDbContext.Products.AddAsync(new Product
            {
                Name = "Test",
                Description = "Test",
                Category = "Test",
                Price = 1,
                Quantity = 1,
                CreatedDateUtc = DateTime.Now,
                UpdatedDateUtc = DateTime.Now,
            });
              
            await catalogDbContext.SaveChangesAsync();
        }

        public async Task<List<string>> GetAllProductsAsync()
        {
            return await catalogDbContext.Products.Select(x => x.Name).ToListAsync();
        }

        public async Task<Product?> GetByName()
        {
            return await catalogDbContext.Products.Where(x => x.Name == "Test").FirstOrDefaultAsync();
        }

        public async Task Update()
        {
            Product? product = await catalogDbContext.Products.Where(x => x.Name == "Test").FirstOrDefaultAsync();
            product.Name = "UpdateTest";
            catalogDbContext.Update(product);

            await catalogDbContext.SaveChangesAsync();
        }

        public async Task DeleteByName()
        {
            Product? product = await catalogDbContext.Products.Where(x => x.Name == "Test").FirstOrDefaultAsync();
            catalogDbContext.Products.Remove(product);

            await catalogDbContext.SaveChangesAsync();
        }
    }
}
