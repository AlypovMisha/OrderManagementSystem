using CatalogService.Application.DTOs;
using CatalogService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDTO product)
        {
            await _productService.CreateProductAsync(product);
            return Ok(new { message = "Product created successfully." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductDTO product)
        {
            await _productService.UpdateProductAsync(id, product);
            return Ok(new { message = "Product updated successfully." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            await _productService.DeleteProductAsync(id);
            return Ok(new { message = "Product deleted successfully." });
        }

        [HttpPatch("{id}/quantity")]
        public async Task<IActionResult> UpdateProductQuantity(Guid id, [FromBody] int quantity)
        {
            await _productService.UpdateQuantityProductAsync(id, quantity);
            return Ok(new { message = "Quantity updated successfully." });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return Ok(product);
        }

    }
}
