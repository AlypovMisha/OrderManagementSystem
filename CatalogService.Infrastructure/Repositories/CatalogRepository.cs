using CatalogService.Application.Interfaces;
using CatalogService.Core.Entities;
using CatalogService.Infrastructure.ConfigurationDB;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure.Repositories
{
    public class CatalogRepository(CatalogContext catalogContext) : ICatalogRepository
    {
        public async Task CreateProductAsync(Product product)
        {
            await catalogContext.Products.AddAsync(product);
            await catalogContext.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(Product product)
        {
            catalogContext.Remove(product);
            await catalogContext.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await catalogContext.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await catalogContext.Products.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsProductUniqueAsync(string name)
        {
            return !await catalogContext.Products.AnyAsync(x => x.Name == name);
        }

        public async Task SaveChangesAsync()
        {
            await catalogContext.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product updateProduct)
        {
            updateProduct.UpdatedDateUtc = DateTime.UtcNow;
            catalogContext.Products.Update(updateProduct);
            await catalogContext.SaveChangesAsync();
        }
    }
}
