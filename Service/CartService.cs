using Microsoft.EntityFrameworkCore;
using Project.Model;
using Project.Model.DTOs.CartDto;

namespace Project.Service
{
    public class CartService(ApplicationDbContext context)
    {
        private readonly ApplicationDbContext _context = context;

        // Get Cart by UserId
        public async Task<CartDTO> GetCartByUserIdAsync(string userId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return null; // Cart not found
            }

            // Map to CartDTO
            var cartDto = new CartDTO
            {
                Id = cart.Id,
                Items = cart.Items.Select(ci => new CartItemDTO
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity
                }).ToList()
            };

            return cartDto;
        }

        // Add Product to Cart
        public async Task AddProductToCartAsync(string userId, CartItemDTO cartItemDto)
        {
            var cart = await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                // Create a new cart if it doesn't exist
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == cartItemDto.ProductId) ?? throw new Exception("Product not found.");

            // Check if the product already exists in the cart
            var cartItem = cart.Items
                .FirstOrDefault(ci => ci.ProductId == cartItemDto.ProductId);

            if (cartItem == null)
            {
                // Add new item if not already in the cart
                cart.Items.Add(new CartItem
                {
                    ProductId = cartItemDto.ProductId,
                    Quantity = cartItemDto.Quantity
                });
            }
            else
            {
                // Update quantity if product already in the cart
                cartItem.Quantity += cartItemDto.Quantity;
            }

            await _context.SaveChangesAsync();
        }

        // Remove Product from Cart
        public async Task RemoveProductFromCartAsync(string userId, int productId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                throw new Exception("Cart not found.");
            }

            var cartItem = cart.Items.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem != null)
            {
                cart.Items.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Product not found in cart.");
            }
        }
    }
}
