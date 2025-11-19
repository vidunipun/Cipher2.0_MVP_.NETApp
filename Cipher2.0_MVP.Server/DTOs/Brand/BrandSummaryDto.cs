namespace SentimentAnalysis.API.DTOs.Brand;

public class BrandSummaryDto
{
    public string Brand { get; set; } = string.Empty;
    public int ProductCount { get; set; }
}