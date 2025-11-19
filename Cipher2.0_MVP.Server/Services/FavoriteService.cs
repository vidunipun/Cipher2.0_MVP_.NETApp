using Microsoft.EntityFrameworkCore;
using SentimentAnalysis.API.Data;
using SentimentAnalysis.API.Models;

namespace SentimentAnalysis.API.Services;

public class FavoriteService : IFavoriteService
{
    private readonly AppDbContext _db;
    public FavoriteService(AppDbContext db) => _db = db;

    public async Task<List<UserFavorite>> GetFavoritesAsync(string userId)
        => await _db.UserFavorites
            .Where(f => f.UserId == userId)
            .AsNoTracking()
            .ToListAsync();

    public async Task<bool> AddFavoriteAsync(string userId, string productId)
    {
        var exists = await _db.UserFavorites.AnyAsync(f => f.UserId == userId && f.ProductId == productId);
        if (exists) return false;

        var fav = new UserFavorite
        {
            Id = Guid.NewGuid().ToString(),
            UserId = userId,
            ProductId = productId
        };
        await _db.UserFavorites.AddAsync(fav);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveFavoriteAsync(string userId, string productId)
    {
        var fav = await _db.UserFavorites
            .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);

        if (fav == null) return false;

        _db.UserFavorites.Remove(fav);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleFavoriteAsync(string userId, string productId)
    {
        var exists = await _db.UserFavorites.AnyAsync(f => f.UserId == userId && f.ProductId == productId);
        if (exists)
            return await RemoveFavoriteAsync(userId, productId);
        else
            return await AddFavoriteAsync(userId, productId);
    }
}