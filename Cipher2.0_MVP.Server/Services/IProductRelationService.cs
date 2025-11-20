using SentimentAnalysis.API.DTOs.Common;
using SentimentAnalysis.API.DTOs.Product;
using SentimentAnalysis.API.Models;

namespace SentimentAnalysis.API.Services;

public interface IProductRelationService
{
    Task<List<ProductListItemDto>> GetRelatedProductsAsync(string productId);
    Task<object?> GetProductLineWithProductsAsync(string productLineId);
    Task<List<ProductGroup>> GetAllProductGroupsAsync();
    Task<PaginatedResponseDto<ProductListItemDto>> GetProductsByGroupAsync(
        string groupId, int page = 1, int pageSize = 20);
}