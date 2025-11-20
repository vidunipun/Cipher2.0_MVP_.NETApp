using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SentimentAnalysis.API.Data;
using SentimentAnalysis.API.DTOs.Brand;
using SentimentAnalysis.API.DTOs.Product;
using SentimentAnalysis.API.Models;

public class BrandService : IBrandService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public BrandService(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<List<BrandSummaryDto>> GetBrandListAsync(string? q = null)
    {
        var query = _db.Products.AsNoTracking().Where(p => p.Brand != null);
        if (!string.IsNullOrEmpty(q))
            query = query.Where(p => p.Brand!.Contains(q));

        var result = await query
            .GroupBy(p => p.Brand)
            .Select(g => new BrandSummaryDto
            {
                Brand = g.Key!,
                ProductCount = g.Count()
            })
            .ToListAsync();

        return result;
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

        var dtos = _mapper.Map<List<ProductListItemDto>>(products);
        var groupIds = products.Select(p => p.GroupId).Where(g => g != null).Distinct().ToList();

        return new { brand, groups = groupIds, sampleProducts = dtos };
    }

    public async Task<bool> ToggleBrandFavoriteAsync(string brand, string userId)
    {
        var product = await _db.Products.FirstOrDefaultAsync(p => p.Brand == brand);
        if (product == null) return false;

        var exists = await _db.UserFavorites
            .AnyAsync(f => f.UserId == userId && f.ProductId == product.Id);

        if (exists)
        {
            var fav = await _db.UserFavorites.FirstAsync(f => f.UserId == userId && f.ProductId == product.Id);
            _db.UserFavorites.Remove(fav);
            await _db.SaveChangesAsync();
            return false;
        }

        await _db.UserFavorites.AddAsync(new UserFavorite
        {
            Id = Guid.NewGuid().ToString(),
            UserId = userId,
            ProductId = product.Id,
            CreatedAt = DateTime.UtcNow
        });
        await _db.SaveChangesAsync();
        return true;
    }
}