using Microsoft.AspNetCore.Mvc;
using Project.Model.DTOs.ReviewDto;
using Project.Service;

namespace Project.Controllers
{ 
    [ApiController]
        [Route("api/[controller]")]
    public class ReviewsController : Controller
    {
      
       
            private readonly ReviewService _reviewService;

            public ReviewsController(ReviewService reviewService)
            {
                _reviewService = reviewService;
            }

            // GET /api/reviews/product/{productId}
            [HttpGet("product/{productId}")]
            public async Task<IActionResult> GetReviewsForProduct(int productId)
            {
                var reviews = await _reviewService.GetReviewsForProductAsync(productId);
                return Ok(reviews);
            }

            // POST /api/reviews
            [HttpPost]
            public async Task<IActionResult> AddReview([FromBody] ReviewDTO reviewDto)
            {
                await _reviewService.AddReviewAsync(reviewDto);
                return CreatedAtAction(nameof(GetReviewsForProduct), new { productId = reviewDto.ProductId }, reviewDto);
            }
        }
    
}