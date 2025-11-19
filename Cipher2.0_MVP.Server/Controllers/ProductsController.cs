using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SentimentAnalysis.API.Data;
using SentimentAnalysis.API.Models;

namespace SentimentAnalysis.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ProductsController(AppDbContext db) => _db = db;

        // GET /api/products?page=1&pageSize=20&brand=&groupId=&productLineId=&q=
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int pageSize = 20,
            [FromQuery] string? brand = null, [FromQuery] string? groupId = null,
            [FromQuery] string? productLineId = null, [FromQuery] string? q = null)
        {
            var query = _db.Products.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(brand)) query = query.Where(p => p.Brand == brand);
            if (!string.IsNullOrEmpty(groupId)) query = query.Where(p => p.GroupId == groupId);
            if (!string.IsNullOrEmpty(productLineId)) query = query.Where(p => p.ProductLineId == productLineId);
            if (!string.IsNullOrEmpty(q)) query = query.Where(p => p.ProductName!.Contains(q) || p.Description!.Contains(q));

            var total = await query.CountAsync();
            var items = await query
                .OrderBy(p => p.ProductName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new { total, page, pageSize, items });
        }

        // GET /api/products/search?q=...
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string q, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            if (string.IsNullOrWhiteSpace(q)) return BadRequest("q required");
            var query = _db.Products.AsNoTracking().Where(p => p.ProductName!.Contains(q) || p.Description!.Contains(q));
            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return Ok(new { total, page, pageSize, items });
        }

        // GET /api/products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();

            // related pieces
            var productKeywords = await _db.ProductKeywords.Where(pk => pk.ProductId == id).AsNoTracking().ToListAsync();
            var productSP = await _db.ProductSellingPoints.Where(ps => ps.ProductId == id).AsNoTracking().ToListAsync();
            var spIds = productSP.Select(p => p.SellingPointId).Where(s => s != null).Distinct().ToList();
            var sellingPoints = spIds.Any() ? await _db.SellingPoints.Where(s => spIds.Contains(s.Id)).AsNoTracking().ToListAsync() : new List<SellingPoint>();

            var relatedLinks = await _db.RelatedProducts.Where(rp => rp.ProductId == id).AsNoTracking().ToListAsync();
            var relatedIds = relatedLinks.Select(r => r.RelatedProductId).Where(x => x != null).Distinct().ToList();
            var relatedProducts = relatedIds.Any() ? await _db.Products.Where(p => relatedIds.Contains(p.Id)).AsNoTracking().ToListAsync() : new List<Product>();

            var reviews = await _db.Reviews.Where(r => r.ProductId == id).AsNoTracking().ToListAsync();

            return Ok(new
            {
                product,
                keywords = productKeywords,
                sellingPoints,
                relatedProducts,
                reviews
            });
        }

        // GET /api/products/{id}/related
        [HttpGet("{id}/related")]
        public async Task<IActionResult> GetRelated(string id)
        {
            var relatedLinks = await _db.RelatedProducts.Where(rp => rp.ProductId == id).AsNoTracking().ToListAsync();
            var relatedIds = relatedLinks.Select(r => r.RelatedProductId).Where(x => x != null).Distinct().ToList();
            var relatedProducts = relatedIds.Any() ? await _db.Products.Where(p => relatedIds.Contains(p.Id)).AsNoTracking().ToListAsync() : new List<Product>();
            return Ok(relatedProducts);
        }

        // POST /api/products/{id}/favorite  { "userId": "U1" }
        [HttpPost("{id}/favorite")]
        public async Task<IActionResult> ToggleFavorite(string id, [FromBody] FavoriteDto dto)
        {
            if (string.IsNullOrEmpty(dto.UserId)) return BadRequest("userId required");

            var existing = await _db.UserFavorites.FirstOrDefaultAsync(f => f.UserId == dto.UserId && f.ProductId == id);
            if (existing != null)
            {
                _db.UserFavorites.Remove(existing);
                await _db.SaveChangesAsync();
                return NoContent();
            }

            var fav = new UserFavorite { Id = Guid.NewGuid().ToString(), UserId = dto.UserId, ProductId = id };
            await _db.UserFavorites.AddAsync(fav);
            await _db.SaveChangesAsync();
            return CreatedAtAction(null, null);
        }

        public record FavoriteDto(string UserId);
    }
}