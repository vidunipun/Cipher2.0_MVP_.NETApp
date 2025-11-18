using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SentimentAnalysis.API.Models
{
    public class Product
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? ProductKey { get; set; }
        public string? ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? Brand { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }

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
        [ForeignKey(nameof(ProductLine))]
        public string? ProductLineId { get; set; }

        [ForeignKey(nameof(ProductLineId))]
        public ProductLine? ProductLine { get; set; }

        [ForeignKey(nameof(ProductGroup))]
        public string? GroupId { get; set; }
        public ProductGroup? ProductGroup { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
        public virtual ICollection<ProductKeyword>? ProductKeywords { get; set; }
        public virtual ICollection<ProductSellingPoint>? ProductSellingPoints { get; set; }
        public virtual ICollection<RelatedProduct>? RelatedProducts { get; set; }
        public virtual ICollection<UserFavorite>? UserFavorites { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
