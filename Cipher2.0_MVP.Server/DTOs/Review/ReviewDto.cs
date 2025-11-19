namespace SentimentAnalysis.API.DTOs.Review;

public class ReviewDto
{
    public string Id { get; set; } = null!;
    public string? UserName { get; set; }
    public string? ReviewText { get; set; }
    public double Rating { get; set; }
    public DateTime CreatedAt { get; set; }
}
