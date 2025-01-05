using CatalogService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.Application.Interfaces
{
    public interface ICatalogRepository
    {
        Task CreateProductAsync(Product product);
        Task<List<Product>> GetAllProductsAsync();
        Task<Product?> GetByIdAsync(Guid id);
        Task UpdateProductAsync(Product product);
        Task DeleteByIdAsync(Guid id);
    }
}
