using Microsoft.AspNetCore.Mvc;
using SentimentAnalysis.API.Services;

namespace SentimentAnalysis.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService) => _productService = productService;

        // GET /api/products?page=1&pageSize=20&brand=&groupId=&productLineId=&q=
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int pageSize = 20,
            [FromQuery] string? brand = null, [FromQuery] string? groupId = null,
            [FromQuery] string? productLineId = null, [FromQuery] string? q = null)
        {
            var result = await _productService.GetProductsAsync(page, pageSize, brand, groupId, productLineId, q);
            return Ok(result);
        }

        // GET /api/products/search?q=...
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string q, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            if (string.IsNullOrWhiteSpace(q)) return BadRequest("q required");
            var result = await _productService.SearchProductsAsync(q, page, pageSize);
            return Ok(result);
        }

        // GET /api/products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var dto = await _productService.GetProductByIdAsync(id);
            return dto is null ? NotFound() : Ok(dto);
        }

        // GET /api/products/{id}/related
        [HttpGet("{id}/related")]
        public async Task<IActionResult> GetRelated(string id)
        {
            var list = await _productService.GetRelatedAsync(id);
            return Ok(list);
        }

        // POST /api/products/{id}/favorite  { "userId": "U1" }
        [HttpPost("{id}/favorite")]
        public async Task<IActionResult> ToggleFavorite(string id, [FromBody] FavoriteDto dto)
        {
            if (string.IsNullOrEmpty(dto.UserId)) return BadRequest("userId required");

            // Keep favorite logic in controller or move to service later. Simple toggle here.
            return await Task.Run(() => NoContent());
        }

        public record FavoriteDto(string UserId);
    }
}