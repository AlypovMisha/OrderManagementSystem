using CatalogService.Application.DTOs;
using CatalogService.Application.Exceptions;
using CatalogService.Application.Interfaces;
using CatalogService.Core.Entities;
using Microsoft.Extensions.Logging;
using Serilog;

namespace CatalogService.Application.Services
{
    public class ProductService(ICatalogRepository catalogRepository, ILogger<ProductService> logger) : IProductService
    {
        public async Task<bool> CreateProductAsync(ProductDTO productDto)
        {
            if (!await catalogRepository.IsProductUniqueAsync(productDto.Name))
            {
                logger.LogWarning($"Product name already exists: {productDto.Name}");
                throw new ExceptionNameAlreadyExists();
            }

            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Quantity = productDto.Quantity,
                Category = productDto.Category,
                CreatedDateUtc = DateTime.UtcNow,
                UpdatedDateUtc = DateTime.UtcNow
            };

            await catalogRepository.CreateProductAsync(product);
            return true;
        }

        public async Task<bool> UpdateProductAsync(Guid id, ProductDTO productDto)
        {
            var product = new Product()
            {
                Id = id,
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Quantity = productDto.Quantity,
                Category = productDto.Category,
                UpdatedDateUtc = DateTime.UtcNow
            };

            return await catalogRepository.UpdateProductAsync(id, product);
        }
    }
}
