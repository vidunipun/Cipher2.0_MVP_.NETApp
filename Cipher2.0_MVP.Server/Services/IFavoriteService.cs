
using static SentimentAnalysis.API.Controllers.UserFavoritesController;
using SentimentAnalysis.API.DTOs.Favorite;
public interface IFavoriteService
{
    Task<List<FavoriteDto>> GetFavoritesAsync(string userId);
    Task<bool> AddFavoriteAsync(string userId, string productId);
    Task<bool> RemoveFavoriteAsync(string userId, string productId);
    Task<bool> ToggleFavoriteAsync(string userId, string productId);
}