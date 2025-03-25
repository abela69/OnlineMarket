using Microsoft.EntityFrameworkCore;
using Project.Model.DTOs.ProductDto;
using Project.Model;
using Project.Model.DTOs.ReviewDto;

namespace Project.Service
{
    public class ProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all products with basic details
        public async Task<List<ProductDTO>> GetAllProductsAsync()
        {
            return await _context.Products
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Rating = p.Rating
                }).ToListAsync();
        }

        // Get product details by ID, including average rating
        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            var product = await _context.Products
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return null; // Return null if product not found
            }

            var reviews = await _context.Reviews
                .Where(r => r.ProductId == id)
                .ToListAsync();

            decimal averageRating = reviews.Any()
                ? (decimal)reviews.Average(r => r.Rating)
                : 0;

            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Rating = (double)averageRating // Set the calculated average rating here
            };
        }

        // Add a new product
        public async Task AddProductAsync(ProductDTO productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Rating = productDto.Rating // This rating will be 0 initially, can be updated with reviews later
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        // Add a new review to a product and update its average rating
        public async Task AddReviewAsync(int productId, string userId, ReviewDTO reviewDto)
        {
            // Create the new review from the provided DTO
            var review = new Review
            {
                ProductId = productId,
                UserId = userId,
                Rating = reviewDto.Rating, // Convert from int to decimal for database storage
                Comment = reviewDto.Comment
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            // Fetch the product and recalculate its average rating
            var product = await _context.Products
                .Where(p => p.Id == productId)
                .FirstOrDefaultAsync();

            if (product != null)
            {
                var reviews = await _context.Reviews
                    .Where(r => r.ProductId == productId)
                    .ToListAsync();

                // Calculate the new average rating for the product
                double averageRating = reviews.Any()
                    ? (double)reviews.Average(r => r.Rating)
                    : 0.0;

                product.Rating = averageRating;
                await _context.SaveChangesAsync(); // Save the updated product rating
            }
        }
    }
}
