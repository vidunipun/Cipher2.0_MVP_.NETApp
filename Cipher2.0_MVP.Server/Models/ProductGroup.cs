using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SentimentAnalysis.API.Models
{
    public class ProductGroup
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? GroupName { get; set; } 
        public string? Description { get; set; }

        public virtual ICollection<Product>? Products { get; set; }
    }
}
