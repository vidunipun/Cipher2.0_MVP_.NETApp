using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SentimentAnalysis.API.Data;

namespace SentimentAnalysis.API.Controllers
{
    [ApiController]
    [Route("api/brands")]
    public class BrandsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public BrandsController(AppDbContext db) => _db = db;

        // GET /api/brands
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] string? q = null)
        {
            var query = _db.Products.AsNoTracking().Where(p => p.Brand != null);
            if (!string.IsNullOrEmpty(q)) query = query.Where(p => p.Brand!.Contains(q));
            var brands = await query.GroupBy(p => p.Brand).Select(g => new { brand = g.Key, count = g.Count() }).ToListAsync();
            return Ok(brands);
        }

        // GET /api/brands/search?q=
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q)) return BadRequest("q required");
            var list = await _db.Products.Where(p => p.Brand!.Contains(q)).Select(p => p.Brand).Distinct().Take(20).ToListAsync();
            return Ok(list);
        }

        // GET /api/brands/{brand}
        [HttpGet("{brand}")]
        public async Task<IActionResult> Details(string brand)
        {
            var products = await _db.Products.Where(p => p.Brand == brand).AsNoTracking().Take(20).ToListAsync();
            var groups = products.Select(p => p.GroupId).Distinct().ToList();
            return Ok(new { brand, groups, sampleProducts = products });
        }

        // POST /api/brands/{brand}/favorite { "userId": "U1" }
        [HttpPost("{brand}/favorite")]
        public async Task<IActionResult> Favorite(string brand, [FromBody] FavoriteDto dto)
        {
            // Simple brand favorite: store as UserFavorite referencing a product of that brand (demo)
            var product = await _db.Products.FirstOrDefaultAsync(p => p.Brand == brand);
            if (product == null) return NotFound();
            var existing = await _db.UserFavorites.FirstOrDefaultAsync(f => f.UserId == dto.UserId && f.ProductId == product.Id);
            if (existing != null) { _db.UserFavorites.Remove(existing); await _db.SaveChangesAsync(); return NoContent(); }
            await _db.UserFavorites.AddAsync(new Models.UserFavorite { Id = Guid.NewGuid().ToString(), UserId = dto.UserId, ProductId = product.Id });
            await _db.SaveChangesAsync();
            return Created(string.Empty, null);
        }

        public record FavoriteDto(string UserId);
    }
}