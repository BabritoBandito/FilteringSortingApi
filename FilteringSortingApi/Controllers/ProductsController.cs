using FilteringSortingApi.Data;
using FilteringSortingApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilteringSortingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] ProductFilter filter)
        {
            IQueryable<Product> query = _context.Products
                .Include(p => p.Category);

            if (filter.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == filter.CategoryId.Value);
            }
            else if (!string.IsNullOrEmpty(filter.CategoryName))
            {
                query = query.Where(p => p.Category.Name == filter.CategoryName);
            }

            if (filter.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= filter.MinPrice.Value);
            }

            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= filter.MaxPrice.Value);
            }

            if (filter.InStock.HasValue)
            {
                query = query.Where(p => p.InStock == filter.InStock.Value);
            }

            if (!string.IsNullOrEmpty(filter.NameContains))
            {
                query = query.Where(p => p.Name.Contains(filter.NameContains));
            }

            if (filter.CreatedAfter.HasValue)
            {
                query = query.Where(p => p.CreatedDate >= filter.CreatedAfter.Value);
            }

            if (filter.CreatedBefore.HasValue)
            {
                query = query.Where(p => p.CreatedDate <= filter.CreatedBefore.Value);
            }

            if (!string.IsNullOrEmpty(filter.SortBy))
            {
                string direction = filter.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase)
                    ? "desc"
                    : "asc";

                query = filter.SortBy switch
                {
                    "Price" => direction == "asc"
                        ? query.OrderBy(p => p.Price)
                        : query.OrderByDescending(p => p.Price),
                    "CreatedDate" => direction == "asc"
                        ? query.OrderBy(p => p.CreatedDate)
                        : query.OrderByDescending(p => p.CreatedDate),
                    "Name" => direction == "asc"
                        ? query.OrderBy(p => p.Name)
                        : query.OrderByDescending(p => p.Name),
                    "Category.Name" => direction == "asc"
                        ? query.OrderBy(p => p.Category.Name)
                        : query.OrderByDescending(p => p.Category.Name),
                    _ => query
                };
            }

            var products = await query.ToListAsync();

            var result = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CreatedDate = p.CreatedDate,
                InStock = p.InStock,
                Category = p.Category == null ? new CategoryDto() : new CategoryDto
                {
                    Id = p.Category.Id,
                    Name = p.Category.Name,
                    Description = p.Category.Description
                }
            });

            return Ok(result);
        }

    }
}
