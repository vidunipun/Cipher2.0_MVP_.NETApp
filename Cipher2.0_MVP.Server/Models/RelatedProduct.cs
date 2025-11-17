namespace SentimentAnalysis.API.Models
{
    public class RelatedProduct
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int RelatedProductId { get; set; }
        public Product Related { get; set; }
    }
}
