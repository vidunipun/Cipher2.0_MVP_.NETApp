namespace SentimentAnalysis.API.Models
{
    public class ProductSellingPoint
    {
        public int ProductId { get; set; }
        public required Product Product { get; set; }

        public int SellingPointId { get; set; }
        public required SellingPoint SellingPoint { get; set; }
    }
}
