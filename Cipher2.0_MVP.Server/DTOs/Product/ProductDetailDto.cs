using SentimentAnalysis.API.DTOs.Review;

namespace SentimentAnalysis.API.DTOs.Product;

public class ProductDetailDto
{
    public string Id { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string? Brand { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }
    public double Rating { get; set; }

    // Attributes
    public double Price { get; set; }
    public double Quality { get; set; }
    public double Comfort { get; set; }
    public double Fit { get; set; }

    public List<string> Images { get; set; } = new();
    public List<string> Keywords { get; set; } = new();
    public List<string> SellingPoints { get; set; } = new();
    public List<ProductListItemDto> RelatedProducts { get; set; } = new();
    public List<ReviewDto> Reviews { get; set; } = new();

    // Sentiment
    public int SentPositive { get; set; }
    public int SentNeutral { get; set; }
    public int SentNegative { get; set; }
    public double AverageSentimentScore { get; set; }
}