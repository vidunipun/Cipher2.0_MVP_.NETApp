using Microsoft.AspNetCore.Mvc;
using SentimentAnalysis.API.Services;

namespace SentimentAnalysis.API.Controllers;

[ApiController]
[Route("api/favorites")]
public class UserFavoritesController : ControllerBase
{
    private readonly IFavoriteService _favoriteService;

    public UserFavoritesController(IFavoriteService favoriteService)
        => _favoriteService = favoriteService;

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetByUser(string userId)
        => Ok(await _favoriteService.GetFavoritesAsync(userId));

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] FavoriteDto dto)
    {
        var added = await _favoriteService.AddFavoriteAsync(dto.UserId, dto.ProductId);
        return added
            ? Created(string.Empty, new { dto.UserId, dto.ProductId })
            : Conflict("Already favorited");
    }

    [HttpDelete]
    public async Task<IActionResult> Remove([FromBody] FavoriteDto dto)
    {
        var removed = await _favoriteService.RemoveFavoriteAsync(dto.UserId, dto.ProductId);
        return removed ? NoContent() : NotFound();
    }

    public record FavoriteDto(string UserId, string ProductId);
}