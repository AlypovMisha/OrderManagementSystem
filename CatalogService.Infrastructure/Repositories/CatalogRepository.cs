using CatalogService.Application.Interfaces;
using CatalogService.Core.Entities;
using CatalogService.Infrastructure.Data;
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

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            Product? product = await catalogContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product != null)
            {
                catalogContext.Products.Remove(product);
                await catalogContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
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

        public async Task<bool> UpdateProductAsync(Guid id, Product updateProduct)
        {
            Product? product = await catalogContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product != null)
            {
                product.Name = updateProduct.Name;
                product.Description = updateProduct.Description;
                product.Price = updateProduct.Price;
                product.Quantity = updateProduct.Quantity;
                product.Category = updateProduct.Category;
                await catalogContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdateQuantityAsync(Guid id, int quantity)
        {
            Product? product = await catalogContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product != null && product.Quantity >= quantity)
            {
                product.Quantity -= quantity;
                await catalogContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
