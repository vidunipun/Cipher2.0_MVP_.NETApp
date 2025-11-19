using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SentimentAnalysis.API.Models
{
    public class ProductKeyword
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string? ProductId { get; set; }
        
        [ForeignKey(nameof(ProductId))]
        public virtual Product? Product { get; set; }
        [Required]
        public string? KeywordId { get; set; }
        [ForeignKey(nameof(KeywordId))]
        public virtual Keyword Keyword { get; set; }
    }
}
