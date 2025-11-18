using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SentimentAnalysis.API.Models
{
    public class UserFavorite
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string? UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        [Required]
        public string? ProductId { get; set; }
        
        [ForeignKey(nameof(ProductId))]
        public virtual Product? Product { get; set; }
    }
}
