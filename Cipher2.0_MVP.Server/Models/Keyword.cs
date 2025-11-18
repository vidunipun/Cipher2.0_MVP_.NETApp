using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SentimentAnalysis.API.Models
{
    public class Keyword
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Word { get; set; }

        public virtual ICollection<ProductKeyword> ProductKeywords { get; set; }

        public static implicit operator Keyword(string v)
        {
            throw new NotImplementedException();
        }
    }
}
