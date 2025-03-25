using Microsoft.AspNetCore.Mvc;
using Project.Model.DTOs.CartDto;
using Project.Service;

namespace Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : Controller
    {

        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        // Get Cart for User
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCart(string userId)
        {
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                return NotFound("Cart not found for this user.");
            }

            return Ok(cart);
        }

        // Add Product to Cart
        [HttpPost("{userId}")]
        public async Task<IActionResult> AddToCart(string userId, [FromBody] CartItemDTO cartItemDto)
        {
            await _cartService.AddProductToCartAsync(userId, cartItemDto);
            return Ok("Product added to cart");
        }

        // Remove Product from Cart
        [HttpDelete("{userId}/{productId}")]
        public async Task<IActionResult> RemoveFromCart(string userId, int productId)
        {
            await _cartService.RemoveProductFromCartAsync(userId, productId);
            return Ok("Product removed from cart");
        }
    }
}
