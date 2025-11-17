using System.Collections.Generic;

namespace SentimentAnalysis.API.Models
{
    public class ProductLine
    {
        public int ProductLineId { get; set; }
        public string ProductLineName { get; set; } // e.g., “Valentine’s Collection”
        public string Description { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
