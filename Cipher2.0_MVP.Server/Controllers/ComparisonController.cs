using Microsoft.AspNetCore.Mvc;

namespace SentimentAnalysis.API.Controllers
{
    [ApiController]
    [Route("api/comparison")]
    public class ComparisonController : ControllerBase
    {
        // Simple demo: per-user in-memory store (not persistent). Replace with DB if needed.
        private static readonly Dictionary<string, List<string>> Store = new();

        // POST /api/comparison/add  { userId, productId }
        [HttpPost("add")]
        public IActionResult Add([FromBody] ItemDto dto)
        {
            var list = Store.GetValueOrDefault(dto.UserId) ?? new List<string>();
            if (!list.Contains(dto.ProductId)) list.Add(dto.ProductId);
            Store[dto.UserId] = list;
            return Ok(list);
        }

        // POST /api/comparison/remove { userId, productId }
        [HttpPost("remove")]
        public IActionResult Remove([FromBody] ItemDto dto)
        {
            if (!Store.TryGetValue(dto.UserId, out var list)) return NotFound();
            list.Remove(dto.ProductId);
            return Ok(list);
        }

        // GET /api/comparison?userId=U1
        [HttpGet]
        public IActionResult Get([FromQuery] string userId)
        {
            var list = Store.GetValueOrDefault(userId) ?? new List<string>();
            return Ok(list);
        }

        public record ItemDto(string UserId, string ProductId);
    }
}