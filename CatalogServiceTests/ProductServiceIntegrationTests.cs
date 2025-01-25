using CatalogService.Application.DTOs;
using CatalogService.Application.Exceptions;
using CatalogService.Application.Services;
using CatalogService.Application.Validations;
using CatalogService.Infrastructure.ConfigurationDB;
using CatalogService.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace CatalogServiceTests
{
    public class ProductServiceIntegrationTests : IDisposable
    {
        private readonly ILogger<ProductService> _logger;
        private readonly CatalogContext _context;
        private readonly ProductService _productService;
        private readonly IValidator<ProductDTO> _productValidator;
        private readonly CatalogRepository _repository;

        public ProductServiceIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<CatalogContext>()
                .UseInMemoryDatabase(databaseName: "TestDbInMemory")
                .Options;

            _context = new CatalogContext(options);
            _logger = Mock.Of<ILogger<ProductService>>();
            _productValidator = new ProductValidator();
            _repository = new CatalogRepository(_context);
            _productService = new ProductService(_repository, _logger, _productValidator);
        }

        [Fact]
        public async Task CreateProductAsync_ShouldAddProductToDatabase()
        {
            // Arrange
            var productDto = new ProductDTO
            {
                Id = Guid.NewGuid(),
                Name = "Test product",
                Description = "Test desctiption",
                Category = "T-shorts",
                Price = 100,
                Quantity = 50,
                CreatedDateUtc = DateTime.UtcNow,
                UpdatedDateUtc = DateTime.UtcNow,
            };

            Guid id = productDto.Id;

            // Act
            await _productService.CreateProductAsync(productDto);

            // Assert
            var product = await _productService.GetProductByIdAsync(id);
            Assert.NotNull(product);
            Assert.Equal(productDto.Name, product.Name);
            Assert.Equal(productDto.Price, product.Price);
        }

        [Fact]
        public async Task CreateProductAsync_ProductNotValid_ShouldntAddProductToDatabase()
        {
            // Arrange
            var productDto = new ProductDTO
            {
                Id = Guid.NewGuid(),
                Name = "",
                Description = "",
                Category = "",
                Price = -1100,
                Quantity = -50,
                CreatedDateUtc = DateTime.UtcNow,
                UpdatedDateUtc = DateTime.UtcNow,
            };

            Guid id = productDto.Id;

            // Act
            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(
                () => _productService.CreateProductAsync(productDto)
            );

            // Assert
            Assert.Contains(exception.Errors, e => e.ErrorMessage == "Name is required.");
            Assert.Contains(exception.Errors, e => e.ErrorMessage == "Price must be greater than zero.");
        }


        [Fact]
        public async Task CreateProductAsync_ProductDuplicat_ShouldntAddProductToDatabase()
        {
            // Arrange
            var productDto = new ProductDTO
            {
                Id = Guid.NewGuid(),
                Name = "Test product",
                Description = "Test desctiption",
                Category = "T-shorts",
                Price = 100,
                Quantity = 50,
                CreatedDateUtc = DateTime.UtcNow,
                UpdatedDateUtc = DateTime.UtcNow,
            };

            await _productService.CreateProductAsync(productDto);


            // Act
            var duplicat = new ProductDTO
            {
                Id = Guid.NewGuid(),
                Name = "Test product",
                Description = "Test desctiption",
                Category = "T-shorts",
                Price = 100,
                Quantity = 50,
                CreatedDateUtc = DateTime.UtcNow,
                UpdatedDateUtc = DateTime.UtcNow,
            };

            ExceptionNameAlreadyExists exception = await Assert.ThrowsAsync<ExceptionNameAlreadyExists>(
                () => _productService.CreateProductAsync(duplicat)
            );

            // Assert
            Assert.Equal("The name of the product being created already exists in the database", exception.Message);
        }

        [Fact]
        public async Task DeleteProductAsync_ShouldProductBeDeletedFromDatabase()
        {
            // Arrange
            var productDto = new ProductDTO
            {
                Id = Guid.NewGuid(),
                Name = "Test product",
                Description = "Test desctiption",
                Category = "T-shorts",
                Price = 100,
                Quantity = 50,
                CreatedDateUtc = DateTime.UtcNow,
                UpdatedDateUtc = DateTime.UtcNow,
            };

            await _productService.CreateProductAsync(productDto);

            Guid id = productDto.Id;

            // Act
            await _productService.DeleteProductAsync(id);

            // Assert
            Assert.Null(await _context.Products.FindAsync(id));
        }

        [Fact]
        public async Task DeleteProductAsync_ProductDosntExist_ShouldntBeDeletedFromDatabase()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            // Act
            ProductNotFoundException exception = await Assert.ThrowsAsync<ProductNotFoundException>(
               () => _productService.DeleteProductAsync(id)
           );

            // Assert
            Assert.Equal($"Product with ID {id} not found.", exception.Message);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task UpdateProductAsync_ProductDosntExist_ShouldntBeUpdatedFromDatabase()
        {
            // Arrange
            var productDto = new ProductDTO
            {
                Id = Guid.NewGuid(),
                Name = "Test product",
                Description = "Test desctiption",
                Category = "T-shorts",
                Price = 100,
                Quantity = 60,
                CreatedDateUtc = DateTime.UtcNow,
                UpdatedDateUtc = DateTime.UtcNow,
            };
            Guid id = Guid.NewGuid();

            // Act
            ProductNotFoundException exception = await Assert.ThrowsAsync<ProductNotFoundException>(
               () => _productService.UpdateProductAsync(id, productDto)
           );

            // Assert
            Assert.Equal($"Product with ID {id} not found.", exception.Message);

        }

        [Fact]
        public async Task UpdateProductAsync_ProductExists_ShouldBeUpdatedInDatabase()
        {
            // Arrange
            var productDto = new ProductDTO
            {
                Id = Guid.NewGuid(),
                Name = "Test product",
                Description = "Test desctiption",
                Category = "T-shorts",
                Price = 100,
                Quantity = 60,
                CreatedDateUtc = DateTime.UtcNow,
                UpdatedDateUtc = DateTime.UtcNow,
            };
            Guid id = productDto.Id;

            await _productService.CreateProductAsync(productDto);

            var updateProductDto = new ProductDTO
            {
                Id = Guid.NewGuid(),
                Name = "New Test product",
                Description = "New Test desctiption",
                Category = "T-shorts",
                Price = 100,
                Quantity = 50,
                CreatedDateUtc = DateTime.UtcNow,
                UpdatedDateUtc = DateTime.UtcNow,
            };

            // Act
            var newProduct = await _productService.UpdateProductAsync(id, updateProductDto);

            // Assert
            Assert.Equal(newProduct.Name, updateProductDto.Name);
            Assert.Equal(newProduct.Description, updateProductDto.Description);
            Assert.Equal(newProduct.Price, updateProductDto.Price);
            Assert.Equal(newProduct.Quantity, updateProductDto.Quantity);

        }

        [Fact]
        public async Task UpdateQuantityProductAsync_ShouldDecreaseQuantity_WhenStockIsSufficient()
        {
            // Arrange
            var productDto = new ProductDTO
            {
                Id = Guid.NewGuid(),
                Name = "Test product",
                Description = "Test desctiption",
                Category = "T-shorts",
                Price = 100,
                Quantity = 60,
                CreatedDateUtc = DateTime.UtcNow,
                UpdatedDateUtc = DateTime.UtcNow,
            };
            int quantity = productDto.Quantity;
            int subtractedNumber = 10;

            await _productService.CreateProductAsync(productDto);
            // Act

            await _productService.UpdateQuantityProductAsync(productDto.Id, subtractedNumber);

            // Assert
            var updatedProduct = await _productService.GetProductByIdAsync(productDto.Id);
            Assert.NotNull(updatedProduct);
            Assert.Equal(quantity - subtractedNumber, updatedProduct.Quantity);
        }

        [Fact]
        public async Task UpdateQuantityProductAsync_ShouldThrowException_WhenStockIsInsufficient()
        {
            // Arrange
            var productDto = new ProductDTO
            {
                Id = Guid.NewGuid(),
                Name = "Test product",
                Description = "Test desctiption",
                Category = "T-shorts",
                Price = 100,
                Quantity = 60,
                CreatedDateUtc = DateTime.UtcNow,
                UpdatedDateUtc = DateTime.UtcNow,
            };

            await _productService.CreateProductAsync(productDto);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<QuantityException>(
                () => _productService.UpdateQuantityProductAsync(productDto.Id, 70)
            );

            Assert.Equal("Insufficient quantity.", exception.Message);
        }

        [Fact]
        public async Task UpdateQuantityProductAsync_ShouldThrowException_WhenProductDoesNotExist()
        {
            // Arrange
            var nonExistingProductId = Guid.NewGuid();
            int quantityToReduce = 5;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ProductNotFoundException>(
                () => _productService.UpdateQuantityProductAsync(nonExistingProductId, quantityToReduce)
            );

            Assert.Equal($"Product with ID {nonExistingProductId} not found.", exception.Message);
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var productDto = new ProductDTO
            {
                Id = Guid.NewGuid(),
                Name = "Test product",
                Description = "Test desctiption",
                Category = "T-shorts",
                Price = 100,
                Quantity = 60,
                CreatedDateUtc = DateTime.UtcNow,
                UpdatedDateUtc = DateTime.UtcNow,
            };

            await _productService.CreateProductAsync(productDto);

            // Act
            var result = await _productService.GetProductByIdAsync(productDto.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productDto.Id, result.Id);
            Assert.Equal(productDto.Name, result.Name);
            Assert.Equal(productDto.Price, result.Price);
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldThrowException_WhenProductDoesNotExist()
        {
            // Arrange
            var nonExistingProductId = Guid.NewGuid();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ProductNotFoundException>(
                () => _productService.GetProductByIdAsync(nonExistingProductId)
            );

            Assert.Equal($"Product with ID {nonExistingProductId} not found.", exception.Message);
        }
    }
}
