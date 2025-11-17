namespace SentimentAnalysis.API.Models
{
    public class ReviewKeyword
    {
        public int ReviewKeywordId { get; set; }
        public int ReviewId { get; set; }
        public Review Review { get; set; }

        public string Keyword { get; set; }
    }
}
