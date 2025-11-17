using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SentimentAnalysis.API.Models
{
    public class Keyword
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int KeywordId { get; set; }
        public string PartitionKey => Id;
        public string Word { get; set; }

        public ICollection<ProductKeyword> ProductKeywords { get; set; }

        public static implicit operator Keyword(string v)
        {
            throw new NotImplementedException();
        }
    }
}
