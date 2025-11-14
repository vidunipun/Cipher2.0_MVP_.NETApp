using System.Collections.Generic;

namespace SentimentAnalysis.API.Models
{
    public class User
    {
        public int UserId { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }

        public required ICollection<UserFavorite> Favorites { get; set; }
    }
}
