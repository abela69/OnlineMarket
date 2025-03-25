using Microsoft.AspNetCore.Mvc;
using Project.Service;

namespace Project.Controllers
{
    public class FiltersController : Controller
    {
        private readonly ProductFilterService _productFilterService;

        public FiltersController(ProductFilterService productFilterService)
        {
            _productFilterService = productFilterService;
        }

        // Endpoint to filter products by name
        [HttpGet("name")]
        public async Task<IActionResult> GetProductsByName([FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Name parameter is required.");
            }

            var products = await _productFilterService.GetProductsByNameAsync(name);

            if (products.Count == 0)
            {
                return NotFound("No products found with the given name.");
            }

            return Ok(products);
        }

        // Endpoint to filter products by ID
        [HttpGet("id")]
        public async Task<IActionResult> GetProductById([FromQuery] int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid product ID.");
            }

            var product = await _productFilterService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound("Product not found.");
            }

            return Ok(product);
        }

        // Endpoint to filter products by price range
        [HttpGet("price")]
        public async Task<IActionResult> GetProductsByPrice([FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice)
        {
            if (!minPrice.HasValue && !maxPrice.HasValue)
            {
                return BadRequest("At least one price parameter (minPrice or maxPrice) is required.");
            }

            var products = await _productFilterService.GetProductsByPriceAsync(minPrice, maxPrice);

            if (products.Count == 0)
            {
                return NotFound("No products found with the given price range.");
            }

            return Ok(products);
        }
    }
}
