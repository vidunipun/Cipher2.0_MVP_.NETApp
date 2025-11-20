using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SentimentAnalysis.API.Data;
using SentimentAnalysis.API.DTOs.Favorite;
using SentimentAnalysis.API.Models;

namespace SentimentAnalysis.API.Services;

public class FavoriteService : IFavoriteService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public FavoriteService(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<List<FavoriteDto>> GetFavoritesAsync(string userId)
    {
        var favs = await _db.UserFavorites
            .Where(f => f.UserId == userId)
            .AsNoTracking()
            .ToListAsync();

        return _mapper.Map<List<FavoriteDto>>(favs);
    }

    public async Task<bool> AddFavoriteAsync(string userId, string productId)
    {
        var exists = await _db.UserFavorites
            .AnyAsync(f => f.UserId == userId && f.ProductId == productId);

        if (exists) return false;

        var fav = new UserFavorite
        {
            Id = Guid.NewGuid().ToString(),
            UserId = userId,
            ProductId = productId,
            CreatedAt = DateTime.UtcNow
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

    // FIXED: This was broken before
    public async Task<bool> ToggleFavoriteAsync(string userId, string productId)
        => await _db.UserFavorites.AnyAsync(f => f.UserId == userId && f.ProductId == productId)
            ? await RemoveFavoriteAsync(userId, productId)
            : await AddFavoriteAsync(userId, productId);
}