using System.Collections.Generic;

namespace SentimentAnalysis.API.Models
{
    public class ProductGroup
    {
        public int ProductGroupId { get; set; }
        public string GroupName { get; set; } 
        public string Description { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
