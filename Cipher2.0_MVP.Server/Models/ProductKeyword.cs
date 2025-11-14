namespace SentimentAnalysis.API.Models
{
    public class ProductKeyword
    {
        public int ProductId { get; set; }
        public required Product Product { get; set; }

        public int KeywordId { get; set; }
        public required Keyword Keyword { get; set; }
    }
}
