using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SentimentAnalysis.API.Models
{
    public class Review
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? ReviewId { get; set; }
        public string? ReviewKey { get; set; }
        public string? ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }

        public string? UserName { get; set; }
        public string? ReviewText { get; set; }
        public string? Sentiment { get; set; }
        public double SentimentScore { get; set; }
        public double Rating { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relationship
        public virtual ICollection<ReviewKeyword>? ReviewKeywords { get; set; }
    }
}
