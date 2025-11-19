namespace SentimentAnalysis.API.Services;

public interface IProductRelationService
{
    Task<List<Models.Product>> GetRelatedProductsAsync(string productId);
    Task<object?> GetProductLineWithProductsAsync(string productLineId);
    Task<List<Models.ProductGroup>> GetAllProductGroupsAsync();
    Task<object> GetProductsByGroupAsync(string groupId, int page = 1, int pageSize = 20);
}