using Microsoft.EntityFrameworkCore;
using Project.Model.DTOs.ProductDto;

namespace Project.Service
{
    public class ProductFilterService
    {

        private readonly ApplicationDbContext _context;

        public ProductFilterService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Filter by Product Name
        public async Task<List<ProductDTO>> GetProductsByNameAsync(string name)
        {
            var products = await _context.Products
                .Where(p => p.Name.Contains(name)) // Filter by name
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Rating = p.Rating
                }).ToListAsync();

            return products;
        }

        // Filter by Product ID
        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            var product = await _context.Products
                .Where(p => p.Id == id) // Filter by ID
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Rating = p.Rating
                }).FirstOrDefaultAsync();

            return product;
        }

        // Filter by Product Price Range
        public async Task<List<ProductDTO>> GetProductsByPriceAsync(decimal? minPrice, decimal? maxPrice)
        {
            var query = _context.Products.AsQueryable();

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value); // Filter by minimum price
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value); // Filter by maximum price
            }

            var products = await query
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Rating = p.Rating
                }).ToListAsync();

            return products;
        }
    }
}
