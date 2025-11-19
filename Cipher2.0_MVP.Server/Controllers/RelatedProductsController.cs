using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SentimentAnalysis.API.Data;

namespace SentimentAnalysis.API.Controllers
{
    [ApiController]
    [Route("api/related-products")]
    public class RelatedProductsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public RelatedProductsController(AppDbContext db) => _db = db;

        // GET /api/related-products/{productId}
        [HttpGet("{productId}")]
        public async Task<IActionResult> Get(string productId)
        {
            var links = await _db.RelatedProducts.Where(r => r.ProductId == productId).AsNoTracking().ToListAsync();
            var ids = links.Select(l => l.RelatedProductId).Where(x => x != null).ToList();
            var items = ids.Any() ? await _db.Products.Where(p => ids.Contains(p.Id)).AsNoTracking().ToListAsync() : new List<Models.Product>();
            return Ok(items);
        }
    }
}