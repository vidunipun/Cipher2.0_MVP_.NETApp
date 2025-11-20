using SentimentAnalysis.API.DTOs.Reference;

public interface IReferenceDataService
{
    Task<List<KeywordDto>> SearchKeywordsAsync(string? q = null);
    Task<List<SellingPointDto>> SearchSellingPointsAsync(string? q = null);
}