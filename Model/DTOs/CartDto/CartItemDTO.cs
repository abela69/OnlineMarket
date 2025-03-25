using System.ComponentModel.DataAnnotations;

namespace Project.Model.DTOs.CartDto
{
    public class CartItemDTO
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100.")]
        public int Quantity { get; set; }
    }
}
