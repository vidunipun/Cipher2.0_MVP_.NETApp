namespace SentimentAnalysis.API.Models
{
    public class ReviewKeyword
    {
        public int ReviewKeywordId { get; set; }
        public int ReviewId { get; set; }
        public required Review Review { get; set; }

        public required string Keyword { get; set; }
    }
}
