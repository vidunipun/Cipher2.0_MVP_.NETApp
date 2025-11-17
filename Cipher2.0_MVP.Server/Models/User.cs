using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SentimentAnalysis.API.Models
{
    public class User
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        // Partition key should match Id (or another string field)
        public string PartitionKey => Id;
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public ICollection<UserFavorite> UserFavorites { get; set; } = new List<UserFavorite>();
    }
}
