namespace SentimentAnalysis.API.DTOs.Product;

public class ProductListItemDto
{
    public string Id { get; set; } = null!;
    public string? ProductKey { get; set; }
    public string? ProductName { get; set; }
    public string? Brand { get; set; }
    public string? Category { get; set; }
    public double Price { get; set; }
    public double Rating { get; set; }
    public List<string>? Images { get; set; }
}
