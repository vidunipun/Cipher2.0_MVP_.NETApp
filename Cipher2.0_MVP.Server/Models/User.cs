using System.Collections.Generic;

namespace SentimentAnalysis.API.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public ICollection<UserFavorite> UserFavorites { get; set; } = new List<UserFavorite>();
    }
}
