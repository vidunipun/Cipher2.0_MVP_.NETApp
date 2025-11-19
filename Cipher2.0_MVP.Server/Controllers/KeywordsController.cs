using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SentimentAnalysis.API.Data;

namespace SentimentAnalysis.API.Controllers
{
    [ApiController]
    [Route("api/keywords")]
    public class KeywordsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public KeywordsController(AppDbContext db) => _db = db;

        // GET /api/keywords?q=
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] string? q = null)
        {
            var query = _db.Keywords.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(q)) query = query.Where(k => k.Word!.Contains(q));
            var list = await query.Take(50).ToListAsync();
            return Ok(list);
        }
    }
}