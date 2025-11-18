using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace SentimentAnalysis.API.Models
{
    public class RelatedProduct
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string? ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual Product? Product { get; set; }

        [Required]
        public string? RelatedProductId { get; set; }
        
        [ForeignKey(nameof(RelatedProductId))]
        public virtual Product? Related { get; set; }
    }
}
