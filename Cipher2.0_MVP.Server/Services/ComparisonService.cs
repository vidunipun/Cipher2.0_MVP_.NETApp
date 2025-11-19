namespace SentimentAnalysis.API.Services;

public class ComparisonService : IComparisonService
{
    private static readonly Dictionary<string, List<string>> Store = new();

    public List<string> GetComparisonList(string userId)
        => Store.GetValueOrDefault(userId) ?? new List<string>();

    public void AddToComparison(string userId, string productId)
    {
        var list = Store.GetValueOrDefault(userId) ?? new();
        if (!list.Contains(productId)) list.Add(productId);
        Store[userId] = list;
    }

    public void RemoveFromComparison(string userId, string productId)
    {
        if (Store.TryGetValue(userId, out var list))
            list.Remove(productId);
    }
}