using CatalogService.Core.Entities;

namespace CatalogService.Application.DTOs
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public DateTime UpdatedDateUtc { get; set; }

        public ProductDTO(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Description = product.Description;
            Category = product.Category;
            Price = product.Price;
            Quantity = product.Quantity;
            CreatedDateUtc = product.CreatedDateUtc;
            UpdatedDateUtc = product.UpdatedDateUtc;
        }

        public ProductDTO()
        {
        }
    }
}
