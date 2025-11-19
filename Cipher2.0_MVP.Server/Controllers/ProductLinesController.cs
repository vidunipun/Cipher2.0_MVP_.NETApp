using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SentimentAnalysis.API.Data;

namespace SentimentAnalysis.API.Controllers
{
    [ApiController]
    [Route("api/product-lines")]
    public class ProductLinesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ProductLinesController(AppDbContext db) => _db = db;

        // GET /api/product-lines/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var pl = await _db.ProductLines.FindAsync(id);
            if (pl == null) return NotFound();
            var products = await _db.Products.Where(p => p.ProductLineId == id).AsNoTracking().Take(50).ToListAsync();
            return Ok(new { productLine = pl, products });
        }
    }
}