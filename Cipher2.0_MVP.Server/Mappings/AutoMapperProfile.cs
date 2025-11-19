using AutoMapper;
using SentimentAnalysis.API.DTOs.Keyword;
using SentimentAnalysis.API.DTOs.Product;
using SentimentAnalysis.API.DTOs.Review;
using SentimentAnalysis.API.DTOs.SellingPoint;
using SentimentAnalysis.API.Models;

namespace SentimentAnalysis.API.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Product, ProductListItemDto>()
            .ForMember(d => d.Images, opt => opt.MapFrom(s => s.Images));

        CreateMap<Product, ProductDetailDto>()
            .ForMember(d => d.Product, opt => opt.MapFrom(s => s));

        CreateMap<Review, ReviewDto>();
        CreateMap<Keyword, KeywordDto>();
        CreateMap<SellingPoint, SellingPointDto>()
            .ForMember(d => d.Point, opt => opt.MapFrom(s => s.Point));

        // Map lists where needed
        CreateMap<Product, ProductListItemDto>();
    }
}
