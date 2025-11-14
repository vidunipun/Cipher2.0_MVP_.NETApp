using System.Collections.Generic;

namespace SentimentAnalysis.API.Models
{
    public class ProductGroup
    {
        public int ProductGroupId { get; set; }
        public required string GroupName { get; set; } 
        public required string Description { get; set; }

        public required ICollection<Product> Products { get; set; }
    }
}
