using Microsoft.AspNetCore.Mvc;
using SentimentAnalysis.API.Services;

[ApiController]
[Route("api/product-groups")]
public class ProductGroupsController : ControllerBase
{
    private readonly IProductRelationService _service;

    public ProductGroupsController(IProductRelationService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> List()
        => Ok(await _service.GetAllProductGroupsAsync());

    [HttpGet("{id}/products")]
    public async Task<IActionResult> Products(string id, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _service.GetProductsByGroupAsync(id, page, pageSize));
}