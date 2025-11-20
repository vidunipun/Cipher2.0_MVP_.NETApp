using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SentimentAnalysis.API.Data;
using SentimentAnalysis.API.DTOs.Review;
using SentimentAnalysis.API.Models;

public class ReviewService : IReviewService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public ReviewService(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<List<ReviewDto>> GetReviewsByProductAsync(string productId)
    {
        var reviews = await _db.Reviews
            .Where(r => r.ProductId == productId)
            .AsNoTracking()
            .ToListAsync();

        return _mapper.Map<List<ReviewDto>>(reviews);
    }

    public async Task<ReviewDto?> GetReviewByIdAsync(string id, string? productId = null)
    {
        var query = _db.Reviews.AsNoTracking();
        if (!string.IsNullOrEmpty(productId))
            query = query.Where(x => x.Id == id && x.ProductId == productId);
        else
            query = query.Where(r => r.Id == id);

        var review = await query.FirstOrDefaultAsync();
        return review == null ? null : _mapper.Map<ReviewDto>(review);
    }

    public async Task<ReviewDto> CreateReviewAsync(Review review)
    {
        review.Id ??= Guid.NewGuid().ToString();
        review.CreatedAt = DateTime.UtcNow;

        await _db.Reviews.AddAsync(review);
        await _db.SaveChangesAsync();

        return _mapper.Map<ReviewDto>(review);
    }
}