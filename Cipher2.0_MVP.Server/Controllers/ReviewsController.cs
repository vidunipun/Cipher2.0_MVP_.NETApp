using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SentimentAnalysis.API.Data;
using SentimentAnalysis.API.Models;

namespace SentimentAnalysis.API.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    public class ReviewsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ReviewsController(AppDbContext db) => _db = db;

        // GET /api/reviews?productId=P1
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] string productId)
        {
            if (string.IsNullOrEmpty(productId)) return BadRequest("productId required");
            var items = await _db.Reviews.Where(r => r.ProductId == productId).AsNoTracking().ToListAsync();
            return Ok(items);
        }

        // POST /api/reviews
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Review dto)
        {
            if (string.IsNullOrEmpty(dto.ProductId)) return BadRequest("ProductId required");
            dto.Id = dto.Id ?? Guid.NewGuid().ToString();
            await _db.Reviews.AddAsync(dto);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        // helper: GET /api/reviews/{id}?productId=...
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id, [FromQuery] string? productId = null)
        {
            if (!string.IsNullOrEmpty(productId))
            {
                var r = await _db.Reviews.FirstOrDefaultAsync(x => x.Id == id && x.ProductId == productId);
                return r == null ? NotFound() : Ok(r);
            }

            // Without productId, attempt a cross-partition find via query (costly)
            var item = await _db.Reviews.FirstOrDefaultAsync(r => r.Id == id);
            return item == null ? NotFound() : Ok(item);
        }
    }
}