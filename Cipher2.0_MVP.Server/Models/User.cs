using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SentimentAnalysis.API.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        [Required]
        public string Email { get; set; }= null!;

        public virtual ICollection<UserFavorite> UserFavorites { get; set; } = new List<UserFavorite>();
    }
}
