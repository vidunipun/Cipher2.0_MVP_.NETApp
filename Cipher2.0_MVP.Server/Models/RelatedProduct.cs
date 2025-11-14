namespace SentimentAnalysis.API.Models
{
    public class RelatedProduct
    {
        public int ProductId { get; set; }
        public required Product Product { get; set; }

        public int RelatedProductId { get; set; }
        public required Product RelatedTo { get; set; }
    }
}
