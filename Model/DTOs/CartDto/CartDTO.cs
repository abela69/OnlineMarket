using System.ComponentModel.DataAnnotations;

namespace Project.Model.DTOs.CartDto
{
    public class CartDTO
    {
        public int Id { get; set; }

        [Required]
        public List<CartItemDTO> Items { get; set; } = new();
    }
}
