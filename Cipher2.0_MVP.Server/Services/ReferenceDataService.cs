using Microsoft.EntityFrameworkCore;
using SentimentAnalysis.API.Data;
using SentimentAnalysis.API.Models;

namespace SentimentAnalysis.API.Services;

public class ReferenceDataService : IReferenceDataService
{
    private readonly AppDbContext _db;
    public ReferenceDataService(AppDbContext db) => _db = db;

    public async Task<List<Keyword>> SearchKeywordsAsync(string? q = null)
    {
        var query = _db.Keywords.AsNoTracking().AsQueryable();
        if (!string.IsNullOrEmpty(q))
            query = query.Where(k => k.Word != null && k.Word.Contains(q));
        return await query.Take(50).ToListAsync();
    }

    public async Task<List<SellingPoint>> SearchSellingPointsAsync(string? q = null)
    {
        var query = _db.SellingPoints.AsNoTracking().AsQueryable();
        if (!string.IsNullOrEmpty(q))
            query = query.Where(s => s.Point != null && s.Point.Contains(q));
        return await query.Take(50).ToListAsync();
    }
}