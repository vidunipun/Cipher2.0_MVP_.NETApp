using SentimentAnalysis.API.DTOs.Review;

namespace SentimentAnalysis.API.DTOs.Product;

public class ProductListItemDto
{
    public string Id { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string? Brand { get; set; }
    public string? Category { get; set; }
    public double Rating { get; set; }
    public int ReviewCount => Reviews?.Count ?? 0;
    public string? ImageUrl => Images?.FirstOrDefault();

    // Populated from navigation
    public List<string> Images { get; set; } = new();
    public List<ReviewDto> Reviews { get; set; } = new(); // optional preview
}