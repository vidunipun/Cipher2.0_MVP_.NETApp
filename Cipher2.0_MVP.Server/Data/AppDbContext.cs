using Microsoft.EntityFrameworkCore;
using SentimentAnalysis.API.Models;

namespace SentimentAnalysis.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Define DbSets for each entity (represents a table)
        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ProductLine> ProductLines { get; set; }
        public DbSet<ProductGroup> productGroups { get; set; }
        public DbSet<ProductKeyword> ProductKeywords { get; set; }
        public DbSet<ProductSellingPoint> ProductSellingPoints { get; set; }
        public DbSet<RelatedProduct> RelatedProducts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserFavorite> UserFavorites { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define composite keys or relationships here
            modelBuilder.Entity<ProductKeyword>()
                .HasKey(pk => new { pk.ProductId, pk.KeywordId });

            modelBuilder.Entity<ProductSellingPoint>()
                .HasKey(ps => new { ps.ProductId, ps.SellingPointId });

            modelBuilder.Entity<UserFavorite>()
                .HasKey(uf => new { uf.UserId, uf.ProductId });

            modelBuilder.Entity<RelatedProduct>()
                .HasKey(rp => new { rp.ProductId, rp.RelatedProductId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
