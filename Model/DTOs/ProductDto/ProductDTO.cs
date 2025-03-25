using System.ComponentModel.DataAnnotations;

namespace Project.Model.DTOs.ProductDto
{
    public class ProductDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 10000)] // Minimum price is 0.01, max 10,000
        public decimal Price { get; set; }

        [Range(0, 5)]
        public double Rating { get; set; } // Optional, defaults to 0
    }
}
