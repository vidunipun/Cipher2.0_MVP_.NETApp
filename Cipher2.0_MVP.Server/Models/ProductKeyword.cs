namespace SentimentAnalysis.API.Models
{
    public class ProductKeyword
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int KeywordId { get; set; }
        public Keyword Keyword { get; set; }
    }
}
