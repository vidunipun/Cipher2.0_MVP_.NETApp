using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SentimentAnalysis.API.Data;

namespace SentimentAnalysis.API.Controllers
{
    [ApiController]
    [Route("api/selling-points")]
    public class SellingPointsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public SellingPointsController(AppDbContext db) => _db = db;

        // GET /api/selling-points?q=
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] string? q = null)
        {
            var query = _db.SellingPoints.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(q)) query = query.Where(s => s.Point!.Contains(q));
            var list = await query.Take(50).ToListAsync();
            return Ok(list);
        }
    }
}