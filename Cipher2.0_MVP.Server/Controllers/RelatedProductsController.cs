using Microsoft.AspNetCore.Mvc;
using SentimentAnalysis.API.Services;

[ApiController]
[Route("api/related-products")]
public class RelatedProductsController : ControllerBase
{
    private readonly IProductRelationService _service;

    public RelatedProductsController(IProductRelationService service) => _service = service;

    [HttpGet("{productId}")]
    public async Task<IActionResult> Get(string productId)
    {
        var products = await _service.GetRelatedProductsAsync(productId);
        return Ok(products);
    }
}