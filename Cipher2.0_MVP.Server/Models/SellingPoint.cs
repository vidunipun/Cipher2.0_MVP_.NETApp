using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SentimentAnalysis.API.Models
{
    public class SellingPoint
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? SellingPointId { get; set; }
        public string? Point { get; set; }

        public virtual ICollection<ProductSellingPoint>? ProductSellingPoints { get; set; }

        public static implicit operator SellingPoint(string v)
        {
            throw new NotImplementedException();
        }
    }
}
