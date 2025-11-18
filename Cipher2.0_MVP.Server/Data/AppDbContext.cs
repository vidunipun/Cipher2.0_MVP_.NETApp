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

        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ProductLine> ProductLines { get; set; }
        public DbSet<ProductGroup> ProductGroups { get; set; }
        public DbSet<ProductKeyword> ProductKeywords { get; set; }
        public DbSet<ProductSellingPoint> ProductSellingPoints { get; set; }
        public DbSet<RelatedProduct> RelatedProducts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserFavorite> UserFavorites { get; set; }
        public DbSet<Keyword> Keywords { get; set; }
        public DbSet<SellingPoint> SellingPoints { get; set; }
        public DbSet<ReviewKeyword> ReviewKeywords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // -------------------------
            // Partition Keys & Containers
            // -------------------------
            modelBuilder.Entity<ProductGroup>()
                .ToContainer("ProductGroups")
                .HasPartitionKey(pg => pg.Id)
                .HasNoDiscriminator();

            modelBuilder.Entity<Product>()
                .ToContainer("Products")
                .HasPartitionKey(p => p.Id)
                .HasNoDiscriminator();

            modelBuilder.Entity<Keyword>()
                .ToContainer("Keywords")
                .HasPartitionKey(k => k.Id)
                .HasNoDiscriminator();

            modelBuilder.Entity<ProductLine>()
                .ToContainer("ProductLines")
                .HasPartitionKey(pl => pl.Id)
                .HasNoDiscriminator();

            modelBuilder.Entity<SellingPoint>()
                .ToContainer("SellingPoints")
                .HasPartitionKey(sp => sp.Id)
                .HasNoDiscriminator();

            modelBuilder.Entity<User>()
                .ToContainer("Users")
                .HasPartitionKey(u => u.Id)
                .HasNoDiscriminator();

            modelBuilder.Entity<Review>()
                .ToContainer("Reviews")
                .HasPartitionKey(r => r.ProductId)
                .HasNoDiscriminator();

            modelBuilder.Entity<ProductKeyword>()
                .ToContainer("ProductKeywords")
                .HasPartitionKey(pk => pk.ProductId)
                .HasNoDiscriminator();

            modelBuilder.Entity<ProductSellingPoint>()
                .ToContainer("ProductSellingPoints")
                .HasPartitionKey(ps => ps.ProductId)
                .HasNoDiscriminator();

            modelBuilder.Entity<UserFavorite>()
                .ToContainer("UserFavorites")
                .HasPartitionKey(uf => uf.UserId)
                .HasNoDiscriminator();

            modelBuilder.Entity<RelatedProduct>()
                .ToContainer("RelatedProducts")
                .HasPartitionKey(rp => rp.ProductId)
                .HasNoDiscriminator();

            modelBuilder.Entity<ReviewKeyword>()
                .ToContainer("ReviewKeywords")
                .HasPartitionKey(rk => rk.ReviewId)
                .HasNoDiscriminator();

            // -------------------------
            // Composite Key & Relationships
            // -------------------------
            modelBuilder.Entity<RelatedProduct>().HasKey(rp => new { rp.ProductId, rp.RelatedProductId });

            // Ignore navigation properties for Cosmos DB (since they're in different containers)
            modelBuilder.Entity<Product>().Ignore(p => p.ProductLine);
            modelBuilder.Entity<Product>().Ignore(p => p.ProductGroup);
            modelBuilder.Entity<Product>().Ignore(p => p.Reviews);
            modelBuilder.Entity<Product>().Ignore(p => p.ProductKeywords);
            modelBuilder.Entity<Product>().Ignore(p => p.ProductSellingPoints);
            modelBuilder.Entity<Product>().Ignore(p => p.RelatedProducts);
            modelBuilder.Entity<Product>().Ignore(p => p.UserFavorites);

            modelBuilder.Entity<Review>().Ignore(r => r.Product);
            modelBuilder.Entity<Review>().Ignore(r => r.ReviewKeywords);

            modelBuilder.Entity<RelatedProduct>().Ignore(rp => rp.Product);
            modelBuilder.Entity<RelatedProduct>().Ignore(rp => rp.Related);

            modelBuilder.Entity<ProductKeyword>().Ignore(pk => pk.Product);
            modelBuilder.Entity<ProductKeyword>().Ignore(pk => pk.Keyword);

            modelBuilder.Entity<ProductSellingPoint>().Ignore(ps => ps.Product);
            modelBuilder.Entity<ProductSellingPoint>().Ignore(ps => ps.SellingPoint);

            modelBuilder.Entity<UserFavorite>().Ignore(uf => uf.User);
            modelBuilder.Entity<UserFavorite>().Ignore(uf => uf.Product);

            modelBuilder.Entity<ReviewKeyword>().Ignore(rk => rk.Review);

            modelBuilder.Entity<User>().Ignore(u => u.UserFavorites);

            modelBuilder.Entity<ProductGroup>().Ignore(pg => pg.Products);

            modelBuilder.Entity<ProductLine>().Ignore(pl => pl.Products);

            modelBuilder.Entity<Keyword>().Ignore(k => k.ProductKeywords);

            modelBuilder.Entity<SellingPoint>().Ignore(sp => sp.ProductSellingPoints);

            base.OnModelCreating(modelBuilder);
        }
    }
}
