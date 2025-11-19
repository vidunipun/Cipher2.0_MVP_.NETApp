using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SentimentAnalysis.API.Data;
using SentimentAnalysis.API.DTOs.Product;
using SentimentAnalysis.API.DTOs.Review;
using SentimentAnalysis.API.Models;
using SentimentAnalysis.API.Services; // <-- Make sure this is here for IProductRelationService

namespace SentimentAnalysis.API.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;
    private readonly IProductRelationService _relationService; // optional but recommended

    public ProductService(AppDbContext db, IMapper mapper, IProductRelationService relationService)
    {
        _db = db;
        _mapper = mapper;
        _relationService = relationService;
    }

    public async Task<object> GetProductsAsync(int page, int pageSize, string? brand, string? groupId, string? productLineId, string? q)
    {
        var query = _db.Products.AsNoTracking().AsQueryable();

        if (!string.IsNullOrEmpty(brand)) query = query.Where(p => p.Brand == brand);
        if (!string.IsNullOrEmpty(groupId)) query = query.Where(p => p.GroupId == groupId);
        if (!string.IsNullOrEmpty(productLineId)) query = query.Where(p => p.ProductLineId == productLineId);
        if (!string.IsNullOrEmpty(q)) query = query.Where(p => p.ProductName!.Contains(q) || p.Description!.Contains(q));

        var total = await query.CountAsync();
        var items = await query
            .OrderBy(p => p.ProductName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var dtos = items.Select(p => _mapper.Map<ProductListItemDto>(p)).ToList();
        return new { total, page, pageSize, items = dtos };
    }

    public async Task<object> SearchProductsAsync(string q, int page, int pageSize)
    {
        var query = _db.Products.AsNoTracking()
            .Where(p => p.ProductName!.Contains(q) || p.Description!.Contains(q));

        var total = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var dtos = items.Select(p => _mapper.Map<ProductListItemDto>(p)).ToList();
        return new { total, page, pageSize, items = dtos };
    }

    public async Task<ProductDetailDto?> GetProductByIdAsync(string id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null) return null;

        var pkTask = _db.ProductKeywords.Where(pk => pk.ProductId == id).AsNoTracking().ToListAsync();
        var psTask = _db.ProductSellingPoints.Where(ps => ps.ProductId == id).AsNoTracking().ToListAsync();
        var reviewsTask = _db.Reviews.Where(r => r.ProductId == id).AsNoTracking().ToListAsync();

        // Use service for related products (cleaner!)
        var relatedProductsTask = _relationService.GetRelatedProductsAsync(id);

        await Task.WhenAll(pkTask, psTask, reviewsTask, relatedProductsTask);

        var dto = _mapper.Map<ProductDetailDto>(product);

        // Keywords
        dto.Keywords = pkTask.Result
            .Select(p => p.KeywordId)
            .Where(x => x != null)
            .ToList()!;

        // Selling Points
        var spIds = psTask.Result
            .Select(x => x.SellingPointId)
            .Where(x => x != null)
            .ToList();

        dto.SellingPoints = spIds.Any()
            ? (await _db.SellingPoints.Where(s => spIds.Contains(s.Id)).ToListAsync())
                .Select(s => s.Point ?? string.Empty)
                .ToList()
            : new List<string>();

        // Related Products
        dto.RelatedProducts = relatedProductsTask.Result
            .Select(p => _mapper.Map<ProductListItemDto>(p))
            .ToList();

        // Reviews
        dto.Reviews = reviewsTask.Result
            .Select(r => _mapper.Map<ReviewDto>(r))
            .ToList();

        return dto;
    }

    public async Task<List<ProductListItemDto>> GetRelatedAsync(string id)
    {
        var related = await _relationService.GetRelatedProductsAsync(id);
        return related.Select(p => _mapper.Map<ProductListItemDto>(p)).ToList();
    }

    // THIS WAS MISSING — NOW FIXED
    public async Task<bool> ToggleFavoriteAsync(string userId, string productId)
    {
        var exists = await _db.UserFavorites
            .AnyAsync(f => f.UserId == userId && f.ProductId == productId);

        if (exists)
        {
            var fav = await _db.UserFavorites
                .FirstAsync(f => f.UserId == userId && f.ProductId == productId);
            _db.UserFavorites.Remove(fav);
            await _db.SaveChangesAsync();
            return false; // removed
        }
        else
        {
            var fav = new UserFavorite
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                ProductId = productId
            };
            await _db.UserFavorites.AddAsync(fav);
            await _db.SaveChangesAsync();
            return true; // added
        }
    }
}