using Project.Model.DTOs.ReviewDto;
using Project.Model;
using Microsoft.EntityFrameworkCore;

namespace Project.Service
{
    public class ReviewService
    {
        private readonly ApplicationDbContext _context;

        public ReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Add a review for a product and update the product's average rating
        public async Task AddReviewAsync(ReviewDTO reviewDto)
        {
            // 1. Create a new Review entity from the DTO
            var review = new Review
            {
                ProductId = reviewDto.ProductId,
                Rating = reviewDto.Rating,
                Comment = reviewDto.Comment
            };

            // 2. Add the review to the database
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            // 3. Update the product's rating based on all reviews for that product
            var product = await _context.Products
                .Where(p => p.Id == reviewDto.ProductId)
                .FirstOrDefaultAsync();

            if (product != null)
            {
                var productReviews = await _context.Reviews
                    .Where(r => r.ProductId == reviewDto.ProductId)
                    .ToListAsync();

                // Calculate the average rating
                product.Rating = (double)productReviews.Average(r => r.Rating);

                // Save the updated product rating
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
            }
        }

        // Get reviews for a product
        public async Task<List<ReviewDTO>> GetReviewsForProductAsync(int productId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.ProductId == productId)
                .Select(r => new ReviewDTO
                {
                    ProductId = r.ProductId,
                    Rating = (int)r.Rating,
                    Comment = r.Comment
                })
                .ToListAsync();

            return reviews;
        }
    }
}
