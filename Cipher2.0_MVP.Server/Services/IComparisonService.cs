namespace SentimentAnalysis.API.Services;

public interface IComparisonService
{
    List<string> GetComparisonList(string userId);
    void AddToComparison(string userId, string productId);
    void RemoveFromComparison(string userId, string productId);
}