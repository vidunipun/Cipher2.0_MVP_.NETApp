using System;
using System.Collections.Generic;

namespace SentimentAnalysis.API.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public required string ProductKey { get; set; }
        public required string ProductName { get; set; }
        public required string Brand { get; set; }
        public required string Category { get; set; }
        public required string Description { get; set; }

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

        // Ratings
        public double Rating { get; set; }

        // Sentiment summary
        public int SentPositive { get; set; }
        public int SentNeutral { get; set; }
        public int SentNegative { get; set; }
        public double AverageSentimentScore { get; set; }

        // Relationships
        public int? ProductLineId { get; set; }
        public required ProductLine ProductLine { get; set; }
        public int? GroupId { get; set; }
        public required ProductGroup ProductGroup { get; set; }
        public required ICollection<Review> Reviews { get; set; }
        public required ICollection<ProductKeyword> ProductKeywords { get; set; }
        public required ICollection<ProductSellingPoint> ProductSellingPoints { get; set; }
        public required ICollection<RelatedProduct> RelatedProducts { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
