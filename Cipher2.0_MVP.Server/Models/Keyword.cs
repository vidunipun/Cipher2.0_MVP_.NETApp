using System.Collections.Generic;

namespace SentimentAnalysis.API.Models
{
    public class Keyword
    {
        public int KeywordId { get; set; }
        public required string Word { get; set; }

        public required ICollection<ProductKeyword> ProductKeywords { get; set; }
    }
}
