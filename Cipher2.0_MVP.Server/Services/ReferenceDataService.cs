using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SentimentAnalysis.API.Data;
using SentimentAnalysis.API.DTOs.Reference;

public class ReferenceDataService : IReferenceDataService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public ReferenceDataService(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<List<KeywordDto>> SearchKeywordsAsync(string? q = null)
    {
        var query = _db.Keywords.AsNoTracking().AsQueryable();
        if (!string.IsNullOrEmpty(q))
            query = query.Where(k => k.Word != null && k.Word.Contains(q));

        var result = await query.Take(50).ToListAsync();
        return _mapper.Map<List<KeywordDto>>(result);
    }

    public async Task<List<SellingPointDto>> SearchSellingPointsAsync(string? q = null)
    {
        var query = _db.SellingPoints.AsNoTracking().AsQueryable();
        if (!string.IsNullOrEmpty(q))
            query = query.Where(s => s.Point != null && s.Point.Contains(q));

        var result = await query.Take(50).ToListAsync();
        return _mapper.Map<List<SellingPointDto>>(result);
    }
}