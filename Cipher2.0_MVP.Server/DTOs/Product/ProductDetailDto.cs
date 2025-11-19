namespace SentimentAnalysis.API.DTOs.Product;

public class ProductDetailDto
{
    public ProductListItemDto Product { get; set; } = null!;
    public List<string>? Keywords { get; set; }
    public List<string>? SellingPoints { get; set; }
    public List<ProductListItemDto>? RelatedProducts { get; set; }
    public List<SentimentAnalysis.API.DTOs.Review.ReviewDto>? Reviews { get; set; }
}
