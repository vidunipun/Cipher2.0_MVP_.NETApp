using Microsoft.AspNetCore.Mvc;
using SentimentAnalysis.API.Services;

namespace SentimentAnalysis.API.Controllers;

[ApiController]
[Route("api/brands")]
public class BrandsController : ControllerBase
{
    private readonly IBrandService _brandService;

    public BrandsController(IBrandService brandService)
        => _brandService = brandService;

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] string? q = null)
        => Ok(await _brandService.GetBrandListAsync(q));

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string q)
        => string.IsNullOrWhiteSpace(q)
            ? BadRequest("q required")
            : Ok(await _brandService.SearchBrandsAsync(q));

    [HttpGet("{brand}")]
    public async Task<IActionResult> Details(string brand)
        => Ok(await _brandService.GetBrandDetailsAsync(brand));

    [HttpPost("{brand}/favorite")]
    public async Task<IActionResult> Favorite(string brand, [FromBody] FavoriteDto dto)
    {
        var added = await _brandService.ToggleBrandFavoriteAsync(brand, dto.UserId);
        return added ? Ok() : NoContent();
    }

    public record FavoriteDto(string UserId);
}