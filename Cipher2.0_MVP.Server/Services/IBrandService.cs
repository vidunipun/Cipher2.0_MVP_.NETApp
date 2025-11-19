namespace SentimentAnalysis.API.Services;

public interface IBrandService
{
    Task<object> GetBrandListAsync(string? q = null);
    Task<List<string>> SearchBrandsAsync(string q);
    Task<object> GetBrandDetailsAsync(string brand);
    Task<bool> ToggleBrandFavoriteAsync(string brand, string userId);
}