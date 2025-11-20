using SentimentAnalysis.API.DTOs.Review;
using SentimentAnalysis.API.Models;

public interface IReviewService
{
    Task<List<ReviewDto>> GetReviewsByProductAsync(string productId);
    Task<ReviewDto?> GetReviewByIdAsync(string id, string? productId = null);
    Task<ReviewDto> CreateReviewAsync(Review review);
}