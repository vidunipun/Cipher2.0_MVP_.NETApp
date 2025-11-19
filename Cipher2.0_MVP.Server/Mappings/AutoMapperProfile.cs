using AutoMapper;
using SentimentAnalysis.API.DTOs.Brand;
using SentimentAnalysis.API.DTOs.Favorite;
using SentimentAnalysis.API.DTOs.Product;
using SentimentAnalysis.API.DTOs.Reference;
using SentimentAnalysis.API.DTOs.Review;
using SentimentAnalysis.API.Models;

namespace SentimentAnalysis.API.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Product → ProductListItemDto
        CreateMap<Product, ProductListItemDto>()
            .ForMember(dest => dest.ImageUrl,
                       opt => opt.MapFrom(src => src.Images != null && src.Images.Any()
                           ? src.Images.First()
                           : null))
            .ForMember(dest => dest.ReviewCount,
                       opt => opt.MapFrom(src => src.Reviews != null ? src.Reviews.Count : 0));

        // Product → ProductDetailDto (ignore collections that we fill manually)
        CreateMap<Product, ProductDetailDto>()
            .ForMember(dest => dest.Keywords, opt => opt.Ignore())
            .ForMember(dest => dest.SellingPoints, opt => opt.Ignore())
            .ForMember(dest => dest.RelatedProducts, opt => opt.Ignore())
            .ForMember(dest => dest.Reviews, opt => opt.Ignore());

        // Review → ReviewDto
        CreateMap<Review, ReviewDto>();

        // Reference data
        CreateMap<Keyword, KeywordDto>();
        CreateMap<SellingPoint, SellingPointDto>();

        // Favorites
        CreateMap<UserFavorite, FavoriteDto>()
            .ForMember(dest => dest.AddedAt, opt => opt.MapFrom(src => src.CreatedAt));

        // Brand summary (used in BrandsController)
        CreateMap<IGrouping<string?, Product>, BrandSummaryDto>()
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Key ?? "Unknown"))
            .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Count()));
    }
}