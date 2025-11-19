using Microsoft.EntityFrameworkCore;
using SentimentAnalysis.API.Data;
using SentimentAnalysis.API.Models;

namespace SentimentAnalysis.API.Services;

public class ReviewService : IReviewService
{
    private readonly AppDbContext _db;
    public ReviewService(AppDbContext db) => _db = db;

    public async Task<List<Review>> GetReviewsByProductAsync(string productId)
        => await _db.Reviews
            .Where(r => r.ProductId == productId)
            .AsNoTracking()
            .ToListAsync();

    public async Task<Review?> GetReviewByIdAsync(string id, string? productId = null)
    {
        if (!string.IsNullOrEmpty(productId))
            return await _db.Reviews.FirstOrDefaultAsync(x => x.Id == id && x.ProductId == productId);

        return await _db.Reviews.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Review> CreateReviewAsync(Review review)
    {
        review.Id ??= Guid.NewGuid().ToString();
        await _db.Reviews.AddAsync(review);
        await _db.SaveChangesAsync();
        return review;
    }
}