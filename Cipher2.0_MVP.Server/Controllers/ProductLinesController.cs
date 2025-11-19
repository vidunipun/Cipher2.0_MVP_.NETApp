using Microsoft.AspNetCore.Mvc;
using SentimentAnalysis.API.Services;

[ApiController]
[Route("api/product-lines")]
public class ProductLinesController : ControllerBase
{
    private readonly IProductRelationService _service;

    public ProductLinesController(IProductRelationService service) => _service = service;

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var result = await _service.GetProductLineWithProductsAsync(id);
        return result is null ? NotFound() : Ok(result);
    }
}