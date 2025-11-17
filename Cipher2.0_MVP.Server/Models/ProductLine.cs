using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SentimentAnalysis.API.Models
{
    public class ProductLine
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        // Partition key should match Id (or another string field)
        public string PartitionKey => Id;
        public string ProductLineId { get; set; }
        public string ProductLineName { get; set; } // e.g., “Valentine’s Collection”
        public string Description { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
