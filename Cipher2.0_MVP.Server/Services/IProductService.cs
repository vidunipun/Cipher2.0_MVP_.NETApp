using SentimentAnalysis.API.DTOs.Product;

namespace SentimentAnalysis.API.Services;

public interface IProductService
{
    Task<object> GetProductsAsync(int page, int pageSize, string? brand, string? groupId, string? productLineId, string? q);
    Task<object> SearchProductsAsync(string q, int page, int pageSize);
    Task<ProductDetailDto?> GetProductByIdAsync(string id);
    Task<List<ProductListItemDto>> GetRelatedAsync(string id);
    Task<bool> ToggleFavoriteAsync(string userId, string productId);
}
