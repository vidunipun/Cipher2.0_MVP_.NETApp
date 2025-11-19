using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SentimentAnalysis.API.Data;
using SentimentAnalysis.API.Models;

namespace SentimentAnalysis.API.Controllers
{
    [ApiController]
    [Route("api/favorites")]
    public class UserFavoritesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public UserFavoritesController(AppDbContext db) => _db = db;

        // GET /api/favorites/{userId}
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetByUser(string userId)
        {
            var favs = await _db.UserFavorites.Where(f => f.UserId == userId).AsNoTracking().ToListAsync();
            return Ok(favs);
        }

        // POST /api/favorites  { userId, productId }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] FavoriteDto dto)
        {
            var exists = await _db.UserFavorites.AnyAsync(f => f.UserId == dto.UserId && f.ProductId == dto.ProductId);
            if (exists) return Conflict("Already favorited");
            var fav = new UserFavorite { Id = Guid.NewGuid().ToString(), UserId = dto.UserId, ProductId = dto.ProductId };
            await _db.UserFavorites.AddAsync(fav);
            await _db.SaveChangesAsync();
            return Created(string.Empty, fav);
        }

        // DELETE /api/favorites  { userId, productId }
        [HttpDelete]
        public async Task<IActionResult> Remove([FromBody] FavoriteDto dto)
        {
            var existing = await _db.UserFavorites.FirstOrDefaultAsync(f => f.UserId == dto.UserId && f.ProductId == dto.ProductId);
            if (existing == null) return NotFound();
            _db.UserFavorites.Remove(existing);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        public record FavoriteDto(string UserId, string ProductId);
    }
}