namespace SentimentAnalysis.API.DTOs.Favorite;

public class FavoriteDto
{
    public string UserId { get; set; } = string.Empty;
    public string ProductId { get; set; } = string.Empty;
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}