using Microsoft.AspNetCore.Mvc;
using SentimentAnalysis.API.Services;

namespace SentimentAnalysis.API.Controllers;

[ApiController]
[Route("api/comparison")]
public class ComparisonController : ControllerBase
{
    private readonly IComparisonService _comparisonService;

    public ComparisonController(IComparisonService comparisonService)
        => _comparisonService = comparisonService;

    [HttpPost("add")]
    public IActionResult Add([FromBody] ItemDto dto)
    {
        _comparisonService.AddToComparison(dto.UserId, dto.ProductId);
        return Ok(_comparisonService.GetComparisonList(dto.UserId));
    }

    [HttpPost("remove")]
    public IActionResult Remove([FromBody] ItemDto dto)
    {
        _comparisonService.RemoveFromComparison(dto.UserId, dto.ProductId);
        return Ok(_comparisonService.GetComparisonList(dto.UserId));
    }

    [HttpGet]
    public IActionResult Get([FromQuery] string userId)
        => Ok(_comparisonService.GetComparisonList(userId));

    public record ItemDto(string UserId, string ProductId);
}