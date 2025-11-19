using SentimentAnalysis.API.Models;

namespace SentimentAnalysis.API.Services;

public interface IReferenceDataService
{
    Task<List<Keyword>> SearchKeywordsAsync(string? q = null);
    Task<List<SellingPoint>> SearchSellingPointsAsync(string? q = null);
}