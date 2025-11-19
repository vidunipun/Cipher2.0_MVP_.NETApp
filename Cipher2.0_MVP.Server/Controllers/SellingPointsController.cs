using Microsoft.AspNetCore.Mvc;
using SentimentAnalysis.API.Services;

namespace SentimentAnalysis.API.Controllers;

[ApiController]
[Route("api/selling-points")]
public class SellingPointsController : ControllerBase
{
    private readonly IReferenceDataService _refService;

    public SellingPointsController(IReferenceDataService refService)
        => _refService = refService;

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] string? q = null)
        => Ok(await _refService.SearchSellingPointsAsync(q));
}