using CatalogService.Application.DTOs;
using CatalogService.Application.Exceptions;
using CatalogService.Application.Services;
using CatalogService.Core.Entities;
using CatalogService.Infrastructure.ConfigurationDB;
using CatalogService.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace CatalogServiceTests
{
    public class CatalogRepositoryTests
    {
        [Fact]
        public async Task CreateProductAsync_ShouldSaveProductToDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CatalogContext>()
                .UseInMemoryDatabase("CreateProductTestDb")
                .Options;

            using var context = new CatalogContext(options);
            var repository = new CatalogRepository(context);
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Test Product",
                Description = "Test Description",
                Category = "Electronics",
                Price = 100.00m,
                Quantity = 10,
                CreatedDateUtc = DateTime.UtcNow,
                UpdatedDateUtc = DateTime.UtcNow
            };

            // Act
            await repository.CreateProductAsync(product);

            // Assert
            var savedProduct = await context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
            Assert.NotNull(savedProduct);
            Assert.Equal("Test Product", savedProduct.Name);
        }

        [Fact]
        public async Task DeleteProductAsync_ShouldRemoveProductFromDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CatalogContext>()
                .UseInMemoryDatabase("RemoveProductTestDb")
                .Options;

            using var context = new CatalogContext(options);
            var repository = new CatalogRepository(context);
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Test Product",
                Description = "Test Description",
                Category = "Electronics",
                Price = 100.00m,
                Quantity = 10,
                CreatedDateUtc = DateTime.UtcNow,
                UpdatedDateUtc = DateTime.UtcNow
            };

            // Act
            await repository.CreateProductAsync(product);
            await repository.DeleteProductAsync(product);

            // Assert
            var savedProduct = await context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
            Assert.Null(savedProduct);
        }

        [Fact]
        public async Task GetAllProductsAsync_ShouldReturnAllProducts()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CatalogContext>()
                .UseInMemoryDatabase("GetAllProductsTestDb")
                .Options;

            using var context = new CatalogContext(options);
            var repository = new CatalogRepository(context);
            var product1 = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Test Product_1",
                Description = "Test Description",
                Category = "Electronics",
                Price = 100.00m,
                Quantity = 10,
                CreatedDateUtc = DateTime.UtcNow,
                UpdatedDateUtc = DateTime.UtcNow
            };

            var product2 = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Test Product_2",
                Description = "Test Description",
                Category = "Electronics",
                Price = 100.00m,
                Quantity = 10,
                CreatedDateUtc = DateTime.UtcNow,
                UpdatedDateUtc = DateTime.UtcNow
            };

            // Act
            await repository.CreateProductAsync(product1);
            await repository.CreateProductAsync(product2);
            

            // Assert
            var savedProducts = await repository.GetAllProductsAsync();
            Assert.NotNull(savedProducts);
            Assert.Equal(2, savedProducts.Count);
            Assert.Equal("Test Product_1", savedProducts[0].Name);
            Assert.Equal("Test Product_2", savedProducts[1].Name);
        }

  

        [Fact]
        public async Task IsProductUniqueAsync_ProductExistsAlready_ShouldBeFalse()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CatalogContext>()
                .UseInMemoryDatabase("IsProductUniqueTestDb")
                .Options;

            using var context = new CatalogContext(options);
            var repository = new CatalogRepository(context);
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Test Product",
                Description = "Test Description",
                Category = "Electronics",
                Price = 100.00m,
                Quantity = 10,
                CreatedDateUtc = DateTime.UtcNow,
                UpdatedDateUtc = DateTime.UtcNow
            };

            var duplicate = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Test Product",
                Description = "Test Description",
                Category = "Electronics",
                Price = 100.00m,
                Quantity = 10,
                CreatedDateUtc = DateTime.UtcNow,
                UpdatedDateUtc = DateTime.UtcNow
            };
            await repository.CreateProductAsync(product);

            // Act
            var isUnique = await repository.IsProductUniqueAsync(duplicate.Name);

            // Assert
            Assert.False(isUnique);
        }

        [Fact]
        public async Task IsProductUniqueAsync_ProductDoesNotExist_ShouldBeTrue()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CatalogContext>()
                .UseInMemoryDatabase("IsProductUniqueTestDb")
                .Options;

            using var context = new CatalogContext(options);
            var repository = new CatalogRepository(context);
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Test Product",
                Description = "Test Description",
                Category = "Electronics",
                Price = 100.00m,
                Quantity = 10,
                CreatedDateUtc = DateTime.UtcNow,
                UpdatedDateUtc = DateTime.UtcNow
            };

            var duplicate = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Another Product",
                Description = "Test Description",
                Category = "Electronics",
                Price = 100.00m,
                Quantity = 10,
                CreatedDateUtc = DateTime.UtcNow,
                UpdatedDateUtc = DateTime.UtcNow
            };
            await repository.CreateProductAsync(product);

            // Act
            var isUnique = await repository.IsProductUniqueAsync(duplicate.Name);

            // Assert
            Assert.True(isUnique);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturns()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CatalogContext>()
                .UseInMemoryDatabase("GetByIdTestDb")
                .Options;

            using var context = new CatalogContext(options);
            var repository = new CatalogRepository(context);
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Test Product",
                Description = "Test Description",
                Category = "Electronics",
                Price = 100.00m,
                Quantity = 10,
                CreatedDateUtc = DateTime.UtcNow,
                UpdatedDateUtc = DateTime.UtcNow
            };

            
            await repository.CreateProductAsync(product);

            // Act
            var product1 = await repository.GetByIdAsync(product.Id);

            // Assert
            Assert.Equal(product, product1);
        }

        [Fact]
        public async Task UpdateProductAsync_ShouldUpdateProductInDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CatalogContext>()
                .UseInMemoryDatabase("UpdateProductTestDb")
                .Options;

            using var context = new CatalogContext(options);
            var repository = new CatalogRepository(context);

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Original Product",
                Description = "Original Description",
                Category = "Electronics",
                Price = 100.00m,
                Quantity = 10,
                CreatedDateUtc = DateTime.UtcNow,
                UpdatedDateUtc = DateTime.UtcNow.AddDays(-1) 
            };

            await repository.CreateProductAsync(product);
            context.ChangeTracker.Clear();

            var updatedProduct = new Product
            {
                Id = product.Id,
                Name = "Updated Product",
                Description = "Updated Description",
                Category = "Updated Category",
                Price = 150.00m,
                Quantity = 20,
                UpdatedDateUtc = product.UpdatedDateUtc 
            };
            // Act
            await repository.UpdateProductAsync(updatedProduct);

            // Assert
            var savedProduct = await repository.GetByIdAsync(product.Id);

            Assert.NotNull(savedProduct);
            Assert.Equal("Updated Product", savedProduct.Name);
            Assert.Equal("Updated Description", savedProduct.Description);
            Assert.Equal("Updated Category", savedProduct.Category);
            Assert.Equal(150.00m, savedProduct.Price);
            Assert.Equal(20, savedProduct.Quantity);
            Assert.True(savedProduct.UpdatedDateUtc > product.UpdatedDateUtc);
        }
    }
}
