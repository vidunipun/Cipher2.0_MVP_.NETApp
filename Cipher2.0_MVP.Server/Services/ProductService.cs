using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SentimentAnalysis.API.Data;
using SentimentAnalysis.API.DTOs.Product;
using SentimentAnalysis.API.DTOs.Review;
using SentimentAnalysis.API.Models;

namespace SentimentAnalysis.API.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public ProductService(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
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
        var query = _db.Products.AsNoTracking().Where(p => p.ProductName!.Contains(q) || p.Description!.Contains(q));
        var total = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        var dtos = items.Select(p => _mapper.Map<ProductListItemDto>(p)).ToList();
        return new { total, page, pageSize, items = dtos };
    }

    public async Task<ProductDetailDto?> GetProductByIdAsync(string id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null) return null;

        var pkTask = _db.ProductKeywords.Where(pk => pk.ProductId == id).AsNoTracking().ToListAsync();
        var psTask = _db.ProductSellingPoints.Where(ps => ps.ProductId == id).AsNoTracking().ToListAsync();
        var relatedTask = _db.RelatedProducts.Where(rp => rp.ProductId == id).AsNoTracking().ToListAsync();
        var reviewsTask = _db.Reviews.Where(r => r.ProductId == id).AsNoTracking().ToListAsync();

        await Task.WhenAll(pkTask, psTask, relatedTask, reviewsTask);

        var dto = _mapper.Map<ProductDetailDto>(product);

        dto.Keywords = pkTask.Result.Select(p => p.KeywordId).Where(x => x != null).ToList()!;

        var spIds = psTask.Result.Select(x => x.SellingPointId).Where(x => x != null).ToList();
        if (spIds.Any())
        {
            var sps = await _db.SellingPoints.Where(s => spIds.Contains(s.Id)).ToListAsync();
            dto.SellingPoints = sps.Select(s => s.Point ?? string.Empty).ToList();
        }
        else
        {
            dto.SellingPoints = new List<string>();
        }

        var relatedIds = relatedTask.Result.Select(r => r.RelatedProductId).Where(x => x != null).ToList();
        if (relatedIds.Any())
        {
            var relatedProducts = await _db.Products.Where(p => relatedIds.Contains(p.Id)).ToListAsync();
            dto.RelatedProducts = relatedProducts.Select(p => _mapper.Map<ProductListItemDto>(p)).ToList();
        }
        else
        {
            dto.RelatedProducts = new List<ProductListItemDto>();
        }

        dto.Reviews = reviewsTask.Result.Select(r => _mapper.Map<ReviewDto>(r)).ToList();

        return dto;
    }

    public async Task<List<ProductListItemDto>> GetRelatedAsync(string id)
    {
        var relatedLinks = await _db.RelatedProducts.Where(rp => rp.ProductId == id).AsNoTracking().ToListAsync();
        var relatedIds = relatedLinks.Select(r => r.RelatedProductId).Where(x => x != null).Distinct().ToList();
        if (!relatedIds.Any()) return new List<ProductListItemDto>();
        var relatedProducts = await _db.Products.Where(p => relatedIds.Contains(p.Id)).AsNoTracking().ToListAsync();
        return relatedProducts.Select(p => _mapper.Map<ProductListItemDto>(p)).ToList();
    }
}
