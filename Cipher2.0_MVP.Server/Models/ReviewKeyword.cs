using System.Text.Json.Serialization;

namespace SentimentAnalysis.API.Models
{
    public class ReviewKeyword
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        // Partition key should match Id (or another string field)
        public string PartitionKey => Id;
        public string ReviewKeywordId { get; set; }
        public string ReviewId { get; set; }
        public Review Review { get; set; }

        public string Keyword { get; set; }
    }
}
