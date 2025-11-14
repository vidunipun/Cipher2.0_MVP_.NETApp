using System;
using System.Collections.Generic;

namespace SentimentAnalysis.API.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public required string ReviewKey { get; set; }
        public int ProductId { get; set; }
        public required Product Product { get; set; }

        public required string UserName { get; set; }
        public required string ReviewText { get; set; }
        public required string Sentiment { get; set; } // positive, neutral, negative
        public double SentimentScore { get; set; }
        public double Rating { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relationship
        public required ICollection<ReviewKeyword> ReviewKeywords { get; set; }
    }
}
