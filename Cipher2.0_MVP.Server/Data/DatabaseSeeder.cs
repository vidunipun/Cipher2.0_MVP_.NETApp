using SentimentAnalysis.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SentimentAnalysis.API.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            // Prevent duplicate seeding
            if (await context.Products.AnyAsync())
                return;

            // -------- PRODUCT LINE --------
            var productLine = new ProductLine
            {
                ProductLineName = "Outdoor Clothing"
            };
            await context.ProductLines.AddAsync(productLine);

            // -------- PRODUCT GROUP --------
            var productGroup = new ProductGroup
            {
                GroupName = "Jackets"
            };
            await context.ProductGroups.AddAsync(productGroup);

            // -------- PRODUCT KEYWORDS --------
            var keywordWarm = new ProductKeyword { Keyword = "Warm" };
            var keywordWaterproof = new ProductKeyword { Keyword = "Waterproof" };
            var keywordWinter = new ProductKeyword { Keyword = "Winter" };
            await context.ProductKeywords.AddRangeAsync(keywordWarm, keywordWaterproof, keywordWinter);

            // -------- PRODUCT SELLING POINTS --------
            var sp1 = new ProductSellingPoint { SellingPoint = "Ultra warm insulation" };
            var sp2 = new ProductSellingPoint { SellingPoint = "Windproof fabric" };
            var sp3 = new ProductSellingPoint { SellingPoint = "Premium waterproof shell" };
            await context.ProductSellingPoints.AddRangeAsync(sp1, sp2, sp3);

            // -------- PRIMARY PRODUCT --------
            var product = new Product
            {
                ProductKey = "PROD-001",
                ProductName = "Arctic Shield Winter Jacket",
                Brand = "FrostWear",
                Category = "Winter Jackets",
                Description = "A premium winter jacket designed for extreme cold weather.",

                Fit = 4.2,
                Comfort = 4.8,
                Functionality = 4.5,
                Aesthetics = 4.7,
                Performance = 4.9,
                Quality = 4.8,
                Workmanship = 4.6,
                Durability = 4.9,
                Price = 3.8,

                Images = new List<string>
                {
                    "https://example.com/images/jacket1.jpg",
                    "https://example.com/images/jacket1_side.jpg"
                },

                Rating = 4.7,

                SentPositive = 120,
                SentNeutral = 25,
                SentNegative = 10,
                AverageSentimentScore = 0.78,

                ProductLine = productLine,
                ProductGroup = productGroup,

                Reviews = new List<Review>(),
                ProductKeywords = new List<ProductKeyword> { keywordWarm, keywordWaterproof },
                ProductSellingPoints = new List<ProductSellingPoint> { sp1, sp2, sp3 },
                RelatedProducts = new List<RelatedProduct>()
            };
            await context.Products.AddAsync(product);

            // -------- REVIEWS --------
            var review1 = new Review
            {
                Product = product,
                Rating = 5,
                SentimentScore = 0.9,
                CreatedAt = DateTime.UtcNow
            };
            var review2 = new Review
            {
                Product = product,
                Rating = 4,
                SentimentScore = 0.6,
                CreatedAt = DateTime.UtcNow
            };
            await context.Reviews.AddRangeAsync(review1, review2);

            // -------- RELATED PRODUCT --------
            var related = new RelatedProduct
            {
                Product = product,
            };
            await context.RelatedProducts.AddAsync(related);

            // -------- SAVE CHANGES ASYNC --------
            await context.SaveChangesAsync();
        }
    }
}
