using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using SentimentAnalysis.API.Data;
using SentimentAnalysis.API.Models;
using User = SentimentAnalysis.API.Models.User;

var builder = WebApplication.CreateBuilder(args);

// 1. EF Core – Cosmos DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseCosmos(
        accountEndpoint: "https://localhost:8081/",
        accountKey: "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
        databaseName: "MVPAppDB"
    )
);

// 2. Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
    var dbName = "MVPAppDB";

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

    // 5. Seed data (async, Cosmos-compatible)
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Ensure EF Core metadata is initialized
    await db.Database.EnsureCreatedAsync();

    // Seed data using try-catch to handle duplicates
    try
    {
        // ProductGroups
        var existingGroup = await db.ProductGroups.FindAsync("1");
        if (existingGroup == null)
        {
            var groups = new List<ProductGroup>
            {
                new() { Id = "1", GroupName = "Jackets" },
                new() { Id = "2", GroupName = "Pants" },
                new() { Id = "3", GroupName = "Shirts" },
                new() { Id = "4", GroupName = "Shoes" }
            };
            await db.ProductGroups.AddRangeAsync(groups);
            await db.SaveChangesAsync();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error seeding ProductGroups: {ex.Message}");
    }

    try
    {
        // Users
        var existingUser = await db.Users.FindAsync("U1");
        if (existingUser == null)
        {
            var users = new List<User>
            {
                new() { Id = "U1", UserName = "Alice", Email = "alice@example.com" },
                new() { Id = "U2", UserName = "Bob", Email = "bob@example.com" }
            };
            await db.Users.AddRangeAsync(users);
            await db.SaveChangesAsync();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error seeding Users: {ex.Message}");
    }

    try
    {
        // Products
        var existingProduct = await db.Products.FindAsync("P1");
        if (existingProduct == null)
        {
            var products = new List<Product>
            {
                new() { Id = "P1", ProductName = "Leather Jacket", GroupId = "1" },
                new() { Id = "P2", ProductName = "Jeans", GroupId = "2" }
            };
            await db.Products.AddRangeAsync(products);
            await db.SaveChangesAsync();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error seeding Products: {ex.Message}");
    }

    try
    {
        // Reviews (partition key = ProductId)
        var existingReview = await db.Reviews.FindAsync("R1");
        if (existingReview == null)
        {
            var reviews = new List<Review>
            {
                new() { Id = "R1", ProductId = "P1", Rating = 5},
                new() { Id = "R2", ProductId = "P2", Rating = 4 }
            };
            await db.Reviews.AddRangeAsync(reviews);
            await db.SaveChangesAsync();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error seeding Reviews: {ex.Message}");
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
