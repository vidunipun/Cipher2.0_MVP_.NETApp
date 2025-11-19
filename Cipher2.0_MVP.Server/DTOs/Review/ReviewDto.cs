namespace SentimentAnalysis.API.DTOs.Review;

public class ReviewDto
{
    public string Id { get; set; } = string.Empty;
    public string ProductId { get; set; } = string.Empty;
    public string? UserName { get; set; }
    public string ReviewText { get; set; } = string.Empty;
    public string Sentiment { get; set; } = string.Empty;
    public double SentimentScore { get; set; }
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; }
}