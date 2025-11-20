using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace SentimentAnalysis.API.Models
{
    public class ProductSellingPoint
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual Product? Product { get; set; }

        public string? SellingPointId { get; set; }
        [ForeignKey(nameof(SellingPointId))]
        public SellingPoint? SellingPoint { get; set; }
    }
}
