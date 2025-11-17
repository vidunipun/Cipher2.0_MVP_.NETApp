using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SentimentAnalysis.API.Models
{
    public class Review
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        // Partition key should match Id (or another string field)
        public string PartitionKey => Id;
        public int ReviewId { get; set; }
        public string ReviewKey { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string UserName { get; set; }
        public string ReviewText { get; set; }
        public string Sentiment { get; set; } // positive, neutral, negative
        public double SentimentScore { get; set; }
        public double Rating { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relationship
        public ICollection<ReviewKeyword> ReviewKeywords { get; set; }
    }
}
