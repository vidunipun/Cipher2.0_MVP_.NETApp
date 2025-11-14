using Microsoft.Azure.Cosmos;
using System.Reflection;

public class CosmosDbService
{
    private readonly Container _container;

    public CosmosDbService(CosmosClient dbClient, string databaseName, string containerName, string partitionKeyPath)
    {
        // Ensure database and container exist
        var database = dbClient.CreateDatabaseIfNotExistsAsync(databaseName).GetAwaiter().GetResult();
        _container = database.Database.CreateContainerIfNotExistsAsync(
            id: containerName,
            partitionKeyPath: partitionKeyPath,
            throughput: 400 // adjust if needed
        ).GetAwaiter().GetResult().Container;
    }

    // Add item with automatic Id generation
    public async Task AddItemAsync<T>(T item)
    {
        var property = typeof(T).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        if (property != null)
        {
            var value = property.GetValue(item);
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                property.SetValue(item, Guid.NewGuid().ToString());
        }

        await _container.CreateItemAsync(item, new PartitionKey((string)GetPartitionKeyValue(item)));
    }

    // Get item by Id
    public async Task<T?> GetItemAsync<T>(string id, string partitionKey)
    {
        try
        {
            var response = await _container.ReadItemAsync<T>(id, new PartitionKey(partitionKey));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return default;
        }
    }

    // Helper to get partition key dynamically
    private object GetPartitionKeyValue<T>(T item)
    {
        // Default to Id if exists
        var property = typeof(T).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
#pragma warning disable CS8603 // Possible null reference return.
        return property?.GetValue(item)?.ToString() ?? null;
#pragma warning restore CS8603 // Possible null reference return.
    }
}
