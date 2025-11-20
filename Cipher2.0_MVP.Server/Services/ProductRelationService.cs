using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SentimentAnalysis.API.Data;
using SentimentAnalysis.API.DTOs.Common;
using SentimentAnalysis.API.DTOs.Product;
using SentimentAnalysis.API.Models;

namespace SentimentAnalysis.API.Services;

public class ProductRelationService : IProductRelationService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public ProductRelationService(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<List<ProductListItemDto>> GetRelatedProductsAsync(string productId)
    {
        var links = await _db.RelatedProducts
            .Where(r => r.ProductId == productId)
            .Select(r => r.RelatedProductId)
            .ToListAsync();

        if (!links.Any()) return new List<ProductListItemDto>();

        var products = await _db.Products
            .Where(p => links.Contains(p.Id))
            .AsNoTracking()
            .ToListAsync();

        return _mapper.Map<List<ProductListItemDto>>(products);
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

        var dtos = _mapper.Map<List<ProductListItemDto>>(products);

        return new { productLine, products = dtos };
    }

    public async Task<List<ProductGroup>> GetAllProductGroupsAsync()
        => await _db.ProductGroups.AsNoTracking().ToListAsync();

    public async Task<PaginatedResponseDto<ProductListItemDto>> GetProductsByGroupAsync(
        string groupId, int page = 1, int pageSize = 20)
    {
        var query = _db.Products.Where(p => p.GroupId == groupId).AsNoTracking();

        var total = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var dtos = _mapper.Map<List<ProductListItemDto>>(items);

        return new PaginatedResponseDto<ProductListItemDto>(page, pageSize, total, dtos);
    }
}