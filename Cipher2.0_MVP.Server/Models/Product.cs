using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SentimentAnalysis.API.Models
{
    public class Product
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        // Partition key should match Id (or another string field)
        public string PartitionKey => Id;
        public int ProductId { get; set; }
        public string ProductKey { get; set; }
        public string ProductName { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }

        // Attributes
        public double Fit { get; set; }
        public double Comfort { get; set; }
        public double Functionality { get; set; }
        public double Aesthetics { get; set; }
        public double Performance { get; set; }
        public double Quality { get; set; }
        public double Workmanship { get; set; }
        public double Durability { get; set; }
        public double Price { get; set; }
        public List<string> Images { get; set; } = new List<string>();

        // Ratings
        public double Rating { get; set; }

        // Sentiment summary
        public int SentPositive { get; set; }
        public int SentNeutral { get; set; }
        public int SentNegative { get; set; }
        public double AverageSentimentScore { get; set; }

        // Relationships
        public int? ProductLineId { get; set; }
        public ProductLine ProductLine { get; set; }
        public int? GroupId { get; set; }
        public ProductGroup ProductGroup { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<ProductKeyword> ProductKeywords { get; set; }
        public ICollection<ProductSellingPoint> ProductSellingPoints { get; set; }
        public ICollection<RelatedProduct> RelatedProducts { get; set; }
        public ICollection<UserFavorite> UserFavorites { get; set; } = new List<UserFavorite>();


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
