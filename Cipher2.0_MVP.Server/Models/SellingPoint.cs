using System.Collections.Generic;

namespace SentimentAnalysis.API.Models
{
    public class SellingPoint
    {
        public int SellingPointId { get; set; }
        public required string Point { get; set; }

        public required ICollection<ProductSellingPoint> ProductSellingPoints { get; set; }
    }
}
