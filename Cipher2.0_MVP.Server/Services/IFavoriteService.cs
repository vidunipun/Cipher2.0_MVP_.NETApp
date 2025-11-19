namespace SentimentAnalysis.API.Services;

public interface IFavoriteService
{
    Task<List<Models.UserFavorite>> GetFavoritesAsync(string userId);
    Task<bool> AddFavoriteAsync(string userId, string productId);
    Task<bool> RemoveFavoriteAsync(string userId, string productId);
    Task<bool> ToggleFavoriteAsync(string userId, string productId); // bonus
}