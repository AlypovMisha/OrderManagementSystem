using CatalogService.Application.DTOs;
using CatalogService.Application.Exceptions;
using CatalogService.Application.Interfaces;
using CatalogService.Application.Services;
using CatalogService.Core.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;

namespace CatalogServiceTests
{
    public class ProductServiceLogicMockTests
    {

        private readonly Mock<ILogger<ProductService>> _loggerMock;
        private readonly Mock<IValidator<ProductDTO>> _productValidatorMock;
        private readonly Mock<ICatalogRepository> _catalogRepositoryMock;
        private readonly ProductService _productService;

        public ProductServiceLogicMockTests()
        {
            _loggerMock = new Mock<ILogger<ProductService>>();
            _productValidatorMock = new Mock<IValidator<ProductDTO>>();
            _catalogRepositoryMock = new Mock<ICatalogRepository>();

            _productService = new ProductService(
                _catalogRepositoryMock.Object,
                _loggerMock.Object,
                _productValidatorMock.Object);
        }

        [Fact]
        public async Task CreateProductAsync_ShouldSucceed_WhenProductIsValid()
        {
            // Arrange
            var validProduct = new ProductDTO
            {
                Id = Guid.NewGuid(),
                Name = "Valid Product",
                Description = "This is a valid product description.",
                Category = "Electronics",
                Price = 1000,
                Quantity = 10,
                CreatedDateUtc = DateTime.UtcNow,
                UpdatedDateUtc = DateTime.UtcNow
            };

            _productValidatorMock
                .Setup(validator => validator.ValidateAsync(It.IsAny<ProductDTO>(), default))
                .ReturnsAsync(new ValidationResult());

            _catalogRepositoryMock
                .Setup(repo => repo.IsProductUniqueAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            await _productService.CreateProductAsync(validProduct);

            // Assert
            _catalogRepositoryMock.Verify(repo => repo.CreateProductAsync(It.IsAny<Product>()), Times.Once);

            _productValidatorMock.Verify(validator => validator.ValidateAsync(It.IsAny<ProductDTO>(), default), Times.Once);
        }

        [Fact]
        public async Task CreateProductAsync_ShouldThrowException_WhenProductIsNotUnique()
        {
            // Arrange
            _catalogRepositoryMock
                .Setup(repo => repo.IsProductUniqueAsync(It.Is<string>(name => name == "Test Product")))
                .ReturnsAsync(false);

            _productValidatorMock
                .Setup(validator => validator.ValidateAsync(It.IsAny<ProductDTO>(), default))
                .ReturnsAsync(new ValidationResult());

            var duplicateProduct = new ProductDTO
            {
                Name = "Test Product",
                Description = "Duplicate Description",
                Category = "Electronics",
                Price = 200.00m,
                Quantity = 5
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ExceptionNameAlreadyExists>(
                () => _productService.CreateProductAsync(duplicateProduct)
            );

            _catalogRepositoryMock.Verify(repo => repo.IsProductUniqueAsync(It.IsAny<string>()), Times.Once);
            Assert.Equal("The name of the product being created already exists in the database", exception.Message);
        }

        [Fact]
        public async Task CreateProductAsync_ShouldThrowValidationException_WhenProductIsInvalid()
        {
            // Arrange
            var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("Name", "Name is required."),
            new ValidationFailure("Price", "Price must be greater than zero.")
        };

            _productValidatorMock
                .Setup(validator => validator.ValidateAsync(It.IsAny<ProductDTO>(), default))
                .ReturnsAsync(new ValidationResult(validationFailures));

            var invalidProduct = new ProductDTO
            {
                Name = "",
                Description = "Valid description",
                Category = "Electronics",
                Price = -10,
                Quantity = 5
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(
                () => _productService.CreateProductAsync(invalidProduct)
            );

            Assert.Contains(exception.Errors, e => e.ErrorMessage == "Name is required.");
            Assert.Contains(exception.Errors, e => e.ErrorMessage == "Price must be greater than zero.");

            _catalogRepositoryMock.Verify(repo => repo.CreateProductAsync(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task DeleteProductAsync_WhenProductIsNotFound()
        {
            // Arrange
            _catalogRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Product?)null);

            var productService = new ProductService(_catalogRepositoryMock.Object, _loggerMock.Object, _productValidatorMock.Object);

            var invalidProduct = new ProductDTO
            {
                Id = Guid.NewGuid(),
                Name = "12421",
                Description = "Valid description",
                Category = "Electronics",
                Price = 10,
                Quantity = 5
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ProductNotFoundException>(
                () => productService.DeleteProductAsync(invalidProduct.Id)
            );

            _catalogRepositoryMock.Verify(repo => repo.DeleteProductAsync(It.IsAny<Product>()), Times.Never);
            Assert.Equal($"Product with ID {invalidProduct.Id} not found.", exception.Message);
        }

        [Fact]
        public async Task DeleteProductAsync_WhenProductIsFound()
        {
            // Arrange
            _catalogRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Product());


            var invalidProduct = new ProductDTO
            {
                Id = Guid.NewGuid(),
                Name = "12421",
                Description = "Valid description",
                Category = "Electronics",
                Price = 10,
                Quantity = 5
            };

            // Act & Assert
            await _productService.DeleteProductAsync(invalidProduct.Id);

            _catalogRepositoryMock.Verify(repo => repo.DeleteProductAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task UpdateProductAsync_ShouldUpdateProduct_WhenProductExists()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var existingProduct = new Product
            {
                Id = productId,
                Name = "Old Name",
                Description = "Old Description",
                Category = "Old Category",
                Price = 100.00m,
                Quantity = 10
            };

            var updatedProductDto = new ProductDTO
            {
                Name = "New Name",
                Description = "New Description",
                Category = "New Category",
                Price = 200.00m,
                Quantity = 20
            };

            _catalogRepositoryMock
                .Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync(existingProduct);

            // Act
            await _productService.UpdateProductAsync(productId, updatedProductDto);

            // Assert
            _catalogRepositoryMock.Verify(repo => repo.UpdateProductAsync(It.Is<Product>(p =>
                p.Name == updatedProductDto.Name &&
                p.Description == updatedProductDto.Description &&
                p.Category == updatedProductDto.Category &&
                p.Price == updatedProductDto.Price &&
                p.Quantity == updatedProductDto.Quantity
            )), Times.Once);
        }

        [Fact]
        public async Task UpdateProductAsync_ShouldThrowProductNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _catalogRepositoryMock
                .Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync((Product?)null);

            var updatedProductDto = new ProductDTO
            {
                Name = "Updated Name",
                Description = "Updated Description",
                Category = "Updated Category",
                Price = 150.00m,
                Quantity = 5
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ProductNotFoundException>(
                () => _productService.UpdateProductAsync(productId, updatedProductDto)
            );

            _catalogRepositoryMock.Verify(repo => repo.UpdateProductAsync(It.IsAny<Product>()), Times.Never);
            Assert.Equal(exception.Message, $"Product with ID {productId} not found.");
        }

        [Fact]
        public async Task UpdateQuantityProductAsync_ShouldUpdateQuantity_WhenProductExistsAndStockIsSufficient()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var existingProduct = new Product
            {
                Id = productId,
                Quantity = 50
            };

            _catalogRepositoryMock
                .Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync(existingProduct);

            int quantityToReduce = 10;

            // Act
            await _productService.UpdateQuantityProductAsync(productId, quantityToReduce);

            // Assert
            _catalogRepositoryMock.Verify(repo => repo.UpdateProductAsync(It.Is<Product>(p =>
                p.Id == existingProduct.Id &&
                p.Quantity == 40
                )), Times.Once);
        }

        [Fact]
        public async Task UpdateQuantityProductAsync_ShouldThrowProductNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = Guid.NewGuid();

            _catalogRepositoryMock
                .Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync((Product?)null);

            int quantityToReduce = 5;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ProductNotFoundException>(
                () => _productService.UpdateQuantityProductAsync(productId, quantityToReduce)
            );


            _catalogRepositoryMock.Verify(repo => repo.UpdateProductAsync(It.IsAny<Product>()), Times.Never);
            Assert.Equal(exception.Message, $"Product with ID {productId} not found.");
        }

        [Fact]
        public async Task UpdateQuantityProductAsync_ShouldThrowQuantityException_WhenStockIsInsufficient()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var existingProduct = new Product
            {
                Id = productId,
                Quantity = 5
            };

            _catalogRepositoryMock
                .Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync(existingProduct);

            int quantityToReduce = 10;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<QuantityException>(
                () => _productService.UpdateQuantityProductAsync(productId, quantityToReduce)
            );

            _catalogRepositoryMock.Verify(repo => repo.UpdateProductAsync(It.IsAny<Product>()), Times.Never);
            Assert.Equal("Insufficient quantity.", exception.Message);
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnProductDTO_WhenProductExists()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var existingProduct = new Product
            {
                Id = productId,
                Name = "Product 1",
                Description = "Description",
                Category = "Category",
                Price = 100.00m,
                Quantity = 10,
                CreatedDateUtc = DateTime.UtcNow.AddDays(-10),
                UpdatedDateUtc = DateTime.UtcNow
            };

            _catalogRepositoryMock
                .Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync(existingProduct);

            // Act
            var result = await _productService.GetProductByIdAsync(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Id, productId);
            Assert.Equal(result.Name, existingProduct.Name);
            Assert.Equal(result.Description, existingProduct.Description);
            Assert.Equal(result.Category, existingProduct.Category);
            Assert.Equal(result.Price, existingProduct.Price);
            Assert.Equal(result.Quantity, existingProduct.Quantity);
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldThrowProductNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = Guid.NewGuid();

            _catalogRepositoryMock
                .Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync((Product?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ProductNotFoundException>(
                () => _productService.GetProductByIdAsync(productId)
            );

            Assert.Equal(exception.Message, $"Product with ID {productId} not found.");
        }
    }
}
