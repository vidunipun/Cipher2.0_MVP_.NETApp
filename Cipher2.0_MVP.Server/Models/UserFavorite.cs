namespace SentimentAnalysis.API.Models
{
    public class UserFavorite
    {
        public int UserId { get; set; }
        public required User User { get; set; }

        public int ProductId { get; set; }
        public required Product Product { get; set; }
    }
}
