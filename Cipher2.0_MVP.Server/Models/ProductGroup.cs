using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SentimentAnalysis.API.Models
{
    public class ProductGroup
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        // Partition key should match Id (or another string field)
        public string PartitionKey => Id;
        public int ProductGroupId { get; set; }
        public string GroupName { get; set; } 
        public string Description { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
