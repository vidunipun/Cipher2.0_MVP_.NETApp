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
        public DbSet<ProductGroup> ProductGroups { get; set; }
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

            // Relationship with Product
            modelBuilder.Entity<ProductKeyword>()
                .HasOne(pk => pk.Product)
                .WithMany(p => p.ProductKeywords)
                .HasForeignKey(pk => pk.ProductId);

            // Relationship with Keyword
            modelBuilder.Entity<ProductKeyword>()
                .HasOne(pk => pk.Keyword)
                .WithMany() // if Keyword doesn’t have a collection of ProductKeywords
                .HasForeignKey(pk => pk.KeywordId);

            modelBuilder.Entity<ProductSellingPoint>()
                .HasKey(ps => new { ps.ProductId, ps.SellingPointId });

            modelBuilder.Entity<ProductSellingPoint>()
                .HasOne(ps => ps.Product)
                .WithMany(p => p.ProductSellingPoints)
                .HasForeignKey(ps => ps.ProductId);

            modelBuilder.Entity<ProductSellingPoint>()
                .HasOne(ps => ps.SellingPoint)
                .WithMany() // or .WithMany(sp => sp.ProductSellingPoints)
                .HasForeignKey(ps => ps.SellingPointId);

            modelBuilder.Entity<UserFavorite>()
                .HasKey(uf => new { uf.UserId, uf.ProductId });

            modelBuilder.Entity<UserFavorite>()
                .HasOne(uf => uf.User)
                .WithMany(u => u.UserFavorites)
                .HasForeignKey(uf => uf.UserId);

            modelBuilder.Entity<UserFavorite>()
                .HasOne(uf => uf.Product)
                .WithMany(p => p.UserFavorites)
                .HasForeignKey(uf => uf.ProductId);

            modelBuilder.Entity<RelatedProduct>()
                .HasKey(rp => new { rp.ProductId, rp.RelatedProductId });

            modelBuilder.Entity<RelatedProduct>()
                .HasOne(rp => rp.Product)
                .WithMany(p => p.RelatedProducts)
                .HasForeignKey(rp => rp.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RelatedProduct>()
                .HasOne(rp => rp.Related)
                .WithMany()
                .HasForeignKey(rp => rp.RelatedProductId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
