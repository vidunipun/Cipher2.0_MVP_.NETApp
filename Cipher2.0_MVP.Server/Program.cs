using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using SentimentAnalysis.API.Data;
using SentimentAnalysis.API.Models;
using User = SentimentAnalysis.API.Models.User;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// 1. EF Core – Cosmos DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseCosmos(
        accountEndpoint: "https://localhost:8081/",
        accountKey: "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
        databaseName: "MVPAppDB_1"
    )
);

// 2. Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// AutoMapper
builder.Services.AddAutoMapper(typeof(SentimentAnalysis.API.Mappings.AutoMapperProfile));

// 3. CosmosClient
builder.Services.AddSingleton(sp =>
{
    return new CosmosClient(
        "https://localhost:8081/",
        "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
        new CosmosClientOptions
        {
            ConnectionMode = ConnectionMode.Gateway,
            HttpClientFactory = () =>
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
                return new HttpClient(handler);
            }
        });
});

var app = builder.Build();

// 4. DATABASE & CONTAINER INITIALIZATION
await using (var scope = app.Services.CreateAsyncScope())
{
    var cosmosClient = scope.ServiceProvider.GetRequiredService<CosmosClient>();
    var dbName = "MVPAppDB_1";

    var databaseResponse = await cosmosClient.CreateDatabaseIfNotExistsAsync(dbName);
    var database = databaseResponse.Database;

    async Task CreateContainerIfNotExists(string containerName, string partitionKeyPath)
    {
        await database.CreateContainerIfNotExistsAsync(
            new ContainerProperties(containerName, partitionKeyPath));
    }

    // Containers
    await CreateContainerIfNotExists("Products", "/id");
    await CreateContainerIfNotExists("Reviews", "/ProductId");
    await CreateContainerIfNotExists("ProductLines", "/id");
    await CreateContainerIfNotExists("ProductGroups", "/id");
    await CreateContainerIfNotExists("Keywords", "/id");
    await CreateContainerIfNotExists("SellingPoints", "/id");
    await CreateContainerIfNotExists("Users", "/id");
    await CreateContainerIfNotExists("ProductKeywords", "/ProductId");
    await CreateContainerIfNotExists("ProductSellingPoints", "/ProductId");
    await CreateContainerIfNotExists("UserFavorites", "/UserId");
    await CreateContainerIfNotExists("RelatedProducts", "/ProductId");
    await CreateContainerIfNotExists("ReviewKeywords", "/ReviewId");

    // 5. Seed data (async, Cosmos-compatible)
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Ensure EF Core metadata is initialized
    await db.Database.EnsureCreatedAsync();

    // Seed ProductGroups
    try
    {
        var existing = await db.ProductGroups.FindAsync("G1");
        if (existing == null)
        {
            var groups = Enumerable.Range(1, 10).Select(i => new ProductGroup
            {
                Id = $"G{i}",
                GroupName = i == 1 ? "Jackets" : $"Group {i}",
                Description = $"Description for group {i}"
            }).ToList();
            await db.ProductGroups.AddRangeAsync(groups);
            await db.SaveChangesAsync();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error seeding ProductGroups: {ex.Message}");
    }

    // Seed ProductLines
    try
    {
        var existing = await db.ProductLines.FindAsync("L1");
        if (existing == null)
        {
            var lines = Enumerable.Range(1, 10).Select(i => new ProductLine
            {
                Id = $"L{i}",
                ProductLineName = i == 1 ? "Outdoor Clothing" : $"Line {i}",
                Description = $"Description for line {i}"
            }).ToList();
            await db.ProductLines.AddRangeAsync(lines);
            await db.SaveChangesAsync();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error seeding ProductLines: {ex.Message}");
    }

    // Seed Keywords
    try
    {
        var existing = await db.Keywords.FindAsync("K1");
        if (existing == null)
        {
            var keywords = Enumerable.Range(1, 10).Select(i => new Keyword
            {
                Id = $"K{i}",
                Word = i switch
                {
                    1 => "Warm",
                    2 => "Waterproof",
                    3 => "Lightweight",
                    4 => "Insulated",
                    5 => "Breathable",
                    6 => "Windproof",
                    7 => "Stretch",
                    8 => "Durable",
                    9 => "Quick-Dry",
                    _ => $"Keyword{i}"
                }
            }).ToList();
            await db.Keywords.AddRangeAsync(keywords);
            await db.SaveChangesAsync();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error seeding Keywords: {ex.Message}");
    }

    // Seed SellingPoints
    try
    {
        var existing = await db.SellingPoints.FindAsync("SP1");
        if (existing == null)
        {
            var sps = Enumerable.Range(1, 10).Select(i => new SellingPoint
            {
                Id = $"SP{i}",
                Point = i switch
                {
                    1 => "Ultra warm insulation",
                    2 => "Seam-sealed waterproofing",
                    3 => "Lightweight construction",
                    4 => "Packable design",
                    5 => "Reinforced stitching",
                    6 => "Adjustable hood",
                    7 => "Vent zippers",
                    8 => "Articulated knees",
                    9 => "Anti-microbial lining",
                    _ => $"Selling point {i}"
                }
            }).ToList();
            await db.SellingPoints.AddRangeAsync(sps);
            await db.SaveChangesAsync();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error seeding SellingPoints: {ex.Message}");
    }

    // Seed Users
    try
    {
        var existing = await db.Users.FindAsync("U1");
        if (existing == null)
        {
            var users = Enumerable.Range(1, 10).Select(i => new User
            {
                Id = $"U{i}",
                UserName = $"User{i}",
                Email = $"user{i}@example.com"
            }).ToList();
            await db.Users.AddRangeAsync(users);
            await db.SaveChangesAsync();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error seeding Users: {ex.Message}");
    }

    // Seed Products (with Images)
    try
    {
        var existing = await db.Products.FindAsync("P1");
        if (existing == null)
        {
            var linesList = await db.ProductLines.ToListAsync();
            var groupsList = await db.ProductGroups.ToListAsync();

            var products = Enumerable.Range(1, 10).Select(i => new Product
            {
                Id = $"P{i}",
                ProductKey = $"PROD-{i:000}",
                ProductId = $"PID{i}",
                ProductName = $"Product {i}",
                Brand = i % 2 == 0 ? "BrandA" : "BrandB",
                Category = i % 3 == 0 ? "Pants" : "Jackets",
                Description = $"Description for product {i}",
                Fit = 4.0 + (i % 5) * 0.1,
                Comfort = 3.5 + (i % 4) * 0.2,
                Functionality = 4.0,
                Aesthetics = 3.8,
                Performance = 4.1,
                Quality = 4.2,
                Workmanship = 4.0,
                Durability = 3.9 + (i % 3) * 0.1,
                Price = 99 + i,
                Images = new List<string>
                {
                    $"https://example.com/images/product{i}_1.jpg",
                    $"https://example.com/images/product{i}_2.jpg"
                },
                Rating = 4.0 + (i % 5) * 0.1,
                SentPositive = 10 * i,
                SentNeutral = i,
                SentNegative = i / 2,
                AverageSentimentScore = 0.5 + (i % 5) * 0.05,
                ProductLineId = linesList[(i - 1) % linesList.Count].Id,
                GroupId = groupsList[(i - 1) % groupsList.Count].Id
            }).ToList();

            await db.Products.AddRangeAsync(products);
            await db.SaveChangesAsync();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error seeding Products: {ex.Message}");
    }

    // Seed ProductKeywords
    try
    {
        var existing = await db.ProductKeywords.FindAsync("PK1-1");
        if (existing == null)
        {
            var keywordsList = await db.Keywords.ToListAsync();
            var pkList = new List<ProductKeyword>();
            for (int i = 1; i <= 10; i++)
            {
                pkList.Add(new ProductKeyword { Id = $"PK{i}-1", ProductId = $"P{i}", KeywordId = keywordsList[(i - 1) % keywordsList.Count].Id });
                pkList.Add(new ProductKeyword { Id = $"PK{i}-2", ProductId = $"P{i}", KeywordId = keywordsList[(i) % keywordsList.Count].Id });
            }
            await db.ProductKeywords.AddRangeAsync(pkList);
            await db.SaveChangesAsync();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error seeding ProductKeywords: {ex.Message}");
    }

    // Seed ProductSellingPoints
    try
    {
        var existing = await db.ProductSellingPoints.FindAsync("PS1-1");
        if (existing == null)
        {
            var spsList = await db.SellingPoints.ToListAsync();
            var psList = new List<ProductSellingPoint>();
            for (int i = 1; i <= 10; i++)
            {
                psList.Add(new ProductSellingPoint { Id = $"PS{i}-1", ProductId = $"P{i}", SellingPointId = spsList[(i - 1) % spsList.Count].Id });
                psList.Add(new ProductSellingPoint { Id = $"PS{i}-2", ProductId = $"P{i}", SellingPointId = spsList[(i) % spsList.Count].Id });
            }
            await db.ProductSellingPoints.AddRangeAsync(psList);
            await db.SaveChangesAsync();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error seeding ProductSellingPoints: {ex.Message}");
    }

    // Seed RelatedProducts
    try
    {
        var existing = await db.RelatedProducts.FindAsync("RP1-1");
        if (existing == null)
        {
            var relatedList = new List<RelatedProduct>();
            for (int i = 1; i <= 10; i++)
            {
                var next = (i % 10) + 1;
                var next2 = ((i + 1) % 10) + 1;
                relatedList.Add(new RelatedProduct { Id = $"RP{i}-1", ProductId = $"P{i}", RelatedProductId = $"P{next}" });
                relatedList.Add(new RelatedProduct { Id = $"RP{i}-2", ProductId = $"P{i}", RelatedProductId = $"P{next2}" });
            }
            await db.RelatedProducts.AddRangeAsync(relatedList);
            await db.SaveChangesAsync();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error seeding RelatedProducts: {ex.Message}");
    }

    // Seed Reviews and ReviewKeywords
    try
    {
        var existing = await db.Reviews.FindAsync("R1");
        if (existing == null)
        {
            var reviews = new List<Review>();
            var reviewKeywords = new List<ReviewKeyword>();
            int rkCounter = 1;
            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 2; j++)
                {
                    var reviewId = $"R{((i - 1) * 2) + j}";
                    var review = new Review
                    {
                        Id = reviewId,
                        ProductId = $"P{i}",
                        UserName = (await db.Users.FindAsync($"U{((i - 1) % 10) + 1}"))?.UserName,
                        ReviewText = $"Review text {reviewId}",
                        Sentiment = j % 2 == 0 ? "Positive" : "Neutral",
                        SentimentScore = 0.5 + (j * 0.1),
                        Rating = 3 + j,
                        CreatedAt = DateTime.UtcNow
                    };
                    reviews.Add(review);

                    reviewKeywords.Add(new ReviewKeyword { Id = $"RK{rkCounter}", ReviewId = reviewId, Keyword = (await db.Keywords.FindAsync($"K{((rkCounter - 1) % 10) + 1}"))?.Word });
                    rkCounter++;
                }
            }
            await db.Reviews.AddRangeAsync(reviews);
            await db.ReviewKeywords.AddRangeAsync(reviewKeywords);
            await db.SaveChangesAsync();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error seeding Reviews: {ex.Message}");
    }

    // Seed UserFavorites
    try
    {
        var existing = await db.UserFavorites.FindAsync("UF1-1");
        if (existing == null)
        {
            var favs = new List<UserFavorite>();
            for (int i = 1; i <= 10; i++)
            {
                favs.Add(new UserFavorite { Id = $"UF{i}-1", UserId = $"U{i}", ProductId = $"P{((i - 1) % 10) + 1}" });
                favs.Add(new UserFavorite { Id = $"UF{i}-2", UserId = $"U{i}", ProductId = $"P{((i) % 10) + 1}" });
            }
            await db.UserFavorites.AddRangeAsync(favs);
            await db.SaveChangesAsync();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error seeding UserFavorites: {ex.Message}");
    }
}

// 6. Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();
