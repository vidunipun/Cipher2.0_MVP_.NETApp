using Microsoft.EntityFrameworkCore;
using SentimentAnalysis.API.Data;
using SentimentAnalysis.API.Models;

namespace SentimentAnalysis.API.Services;

public class ProductRelationService : IProductRelationService
{
    private readonly AppDbContext _db;

    public ProductRelationService(AppDbContext db) => _db = db;

    public async Task<List<Product>> GetRelatedProductsAsync(string productId)
    {
        var links = await _db.RelatedProducts
            .Where(r => r.ProductId == productId)
            .AsNoTracking()
            .Select(r => r.RelatedProductId)
            .ToListAsync();

        if (!links.Any()) return new List<Product>();

        return await _db.Products
            .Where(p => links.Contains(p.Id))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<object?> GetProductLineWithProductsAsync(string productLineId)
    {
        var productLine = await _db.ProductLines.FindAsync(productLineId);
        if (productLine == null) return null;

        var products = await _db.Products
            .Where(p => p.ProductLineId == productLineId)
            .AsNoTracking()
            .Take(50)
            .ToListAsync();

        return new { productLine, products };
    }

    public async Task<List<ProductGroup>> GetAllProductGroupsAsync()
    {
        return await _db.ProductGroups.AsNoTracking().ToListAsync();
    }

    public async Task<object> GetProductsByGroupAsync(string groupId, int page = 1, int pageSize = 20)
    {
        var query = _db.Products.Where(p => p.GroupId == groupId).AsNoTracking();

        var total = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new { total, page, pageSize, items };
    }
}