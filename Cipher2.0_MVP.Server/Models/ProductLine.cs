using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SentimentAnalysis.API.Models
{
    public class ProductLine
    {
        [Key]
        public string? Id { get; set; }
        [Required]
        public string? ProductLineName { get; set; } 
        public string? Description { get; set; }

        public virtual ICollection<Product>? Products { get; set; }
    }
}
