using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SentimentAnalysis.API.Models
{
    public class ReviewKeyword
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();        
        public string? ReviewId { get; set; }
        
        [ForeignKey(nameof(ReviewId))]
        public Review Review { get; set; }

        public string? Keyword { get; set; }
    }
}
