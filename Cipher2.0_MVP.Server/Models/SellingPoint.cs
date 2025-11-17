using System.Collections.Generic;

namespace SentimentAnalysis.API.Models
{
    public class SellingPoint
    {
        public int SellingPointId { get; set; }
        public string Point { get; set; }

        public ICollection<ProductSellingPoint> ProductSellingPoints { get; set; }

        public static implicit operator SellingPoint(string v)
        {
            throw new NotImplementedException();
        }
    }
}
