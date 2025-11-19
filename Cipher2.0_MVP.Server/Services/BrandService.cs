using Microsoft.EntityFrameworkCore;
using SentimentAnalysis.API.Data;
using SentimentAnalysis.API.Models;

namespace SentimentAnalysis.API.Services;

public class BrandService : IBrandService
{
    private readonly AppDbContext _db;
    public BrandService(AppDbContext db) => _db = db;

    public async Task<object> GetBrandListAsync(string? q = null)
    {
        var query = _db.Products.AsNoTracking().Where(p => p.Brand != null);
        if (!string.IsNullOrEmpty(q))
            query = query.Where(p => p.Brand!.Contains(q));

        return await query
            .GroupBy(p => p.Brand)
            .Select(g => new { brand = g.Key, count = g.Count() })
            .ToListAsync();
    }

    public async Task<List<string>> SearchBrandsAsync(string q)
        => await _db.Products
            .Where(p => p.Brand != null && p.Brand.Contains(q))
            .Select(p => p.Brand!)
            .Distinct()
            .Take(20)
            .ToListAsync();

    public async Task<object> GetBrandDetailsAsync(string brand)
    {
        var products = await _db.Products
            .Where(p => p.Brand == brand)
            .AsNoTracking()
            .Take(20)
            .ToListAsync();

        var groupIds = products.Select(p => p.GroupId).Where(g => g != null).Distinct().ToList();

        return new { brand, groups = groupIds, sampleProducts = products };
    }

    public async Task<bool> ToggleBrandFavoriteAsync(string brand, string userId)
    {
        var product = await _db.Products.FirstOrDefaultAsync(p => p.Brand == brand);
        if (product == null) return false;

        var existing = await _db.UserFavorites
            .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == product.Id);

        if (existing != null)
        {
            _db.UserFavorites.Remove(existing);
            await _db.SaveChangesAsync();
            return false; // removed
        }

        await _db.UserFavorites.AddAsync(new UserFavorite
        {
            Id = Guid.NewGuid().ToString(),
            UserId = userId,
            ProductId = product.Id
        });
        await _db.SaveChangesAsync();
        return true; // added
    }
}