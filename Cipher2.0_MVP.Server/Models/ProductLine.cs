using System.Collections.Generic;

namespace SentimentAnalysis.API.Models
{
    public class ProductLine
    {
        public int ProductLineId { get; set; }
        public required string LineName { get; set; } // e.g., “Valentine’s Collection”
        public required string Description { get; set; }

        public required ICollection<Product> Products { get; set; }
    }
}
