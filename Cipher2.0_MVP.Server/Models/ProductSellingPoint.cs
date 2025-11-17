namespace SentimentAnalysis.API.Models
{
    public class ProductSellingPoint
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int SellingPointId { get; set; }
        public SellingPoint SellingPoint { get; set; }
    }
}
