using SentimentAnalysis.API.DTOs.Brand;

public interface IBrandService
{
    Task<List<BrandSummaryDto>> GetBrandListAsync(string? q = null);
    Task<List<string>> SearchBrandsAsync(string q);
    Task<object> GetBrandDetailsAsync(string brand);
    Task<bool> ToggleBrandFavoriteAsync(string brand, string userId);
}