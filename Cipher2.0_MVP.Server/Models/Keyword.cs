using System.Collections.Generic;

namespace SentimentAnalysis.API.Models
{
    public class Keyword
    {
        public int KeywordId { get; set; }
        public string Word { get; set; }

        public ICollection<ProductKeyword> ProductKeywords { get; set; }

        public static implicit operator Keyword(string v)
        {
            throw new NotImplementedException();
        }
    }
}
