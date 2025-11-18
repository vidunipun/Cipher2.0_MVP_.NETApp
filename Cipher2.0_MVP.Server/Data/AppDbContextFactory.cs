using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace SentimentAnalysis.API.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // Load configuration from appsettings.json
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<AppDbContext>();

            // If using Cosmos DB:
            _ = builder.UseCosmos(
                configuration["Cosmos:Endpoint"],
                configuration["Cosmos:Key"],
                configuration["Cosmos:Database"]
            );

            return new AppDbContext(builder.Options);
        }
    }
}
