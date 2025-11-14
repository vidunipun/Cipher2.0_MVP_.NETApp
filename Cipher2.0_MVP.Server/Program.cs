using Microsoft.EntityFrameworkCore;
using SentimentAnalysis.API.Data;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

// SQL Server DbContext for local testing
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cosmos DB configuration
var cosmosConfig = builder.Configuration.GetSection("Cosmos");

// Register CosmosClient
builder.Services.AddSingleton<CosmosClient>(sp =>
{
    return new CosmosClient(
        cosmosConfig["Endpoint"],
        cosmosConfig["Key"],
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

// Register CosmosDbService
builder.Services.AddSingleton<CosmosDbService>(sp =>
{
    var client = sp.GetRequiredService<CosmosClient>();
    var databaseName = cosmosConfig["Database"];
    var containerName = cosmosConfig["Container"];
    var partitionKeyPath = cosmosConfig["PartitionKey"] ?? "/id"; // default to /id

#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.
    return new CosmosDbService(client, databaseName, containerName, partitionKeyPath);
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8604 // Possible null reference argument.
});

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
