namespace SentimentAnalysis.API.Services;

public interface IReviewService
{
    Task<List<Models.Review>> GetReviewsByProductAsync(string productId);
    Task<Models.Review?> GetReviewByIdAsync(string id, string? productId = null);
    Task<Models.Review> CreateReviewAsync(Models.Review review);
}