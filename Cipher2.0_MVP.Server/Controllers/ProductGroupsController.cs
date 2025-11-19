using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SentimentAnalysis.API.Data;

namespace SentimentAnalysis.API.Controllers
{
    [ApiController]
    [Route("api/product-groups")]
    public class ProductGroupsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ProductGroupsController(AppDbContext db) => _db = db;

        // GET /api/product-groups
        [HttpGet]
        public async Task<IActionResult> List() => Ok(await _db.ProductGroups.AsNoTracking().ToListAsync());

        // GET /api/product-groups/{id}/products
        [HttpGet("{id}/products")]
        public async Task<IActionResult> Products(string id, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var q = _db.Products.Where(p => p.GroupId == id).AsNoTracking();
            var total = await q.CountAsync();
            var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return Ok(new { total, page, pageSize, items });
        }
    }
}