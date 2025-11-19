namespace SentimentAnalysis.API.DTOs.Common;

public record PaginatedResponseDto<T>(
    int Page,
    int PageSize,
    int Total,
    IReadOnlyList<T> Items);