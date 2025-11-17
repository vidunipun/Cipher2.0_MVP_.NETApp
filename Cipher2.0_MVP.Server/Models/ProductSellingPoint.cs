using System.Text.Json.Serialization;

namespace SentimentAnalysis.API.Models
{
    public class ProductSellingPoint
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        // Partition key should match Id (or another string field)
        public string PartitionKey => Id;
        public string ProductId { get; set; }
        public Product Product { get; set; }

        public int SellingPointId { get; set; }
        public SellingPoint SellingPoint { get; set; }
    }
}
