using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SentimentAnalysis.API.Models
{
    public class SellingPoint
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        // Partition key should match Id (or another string field)
        public string PartitionKey => Id;
        public int SellingPointId { get; set; }
        public string Point { get; set; }

        public ICollection<ProductSellingPoint> ProductSellingPoints { get; set; }

        public static implicit operator SellingPoint(string v)
        {
            throw new NotImplementedException();
        }
    }
}
