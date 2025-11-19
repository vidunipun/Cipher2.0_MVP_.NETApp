using Microsoft.AspNetCore.Mvc;
using SentimentAnalysis.API.Services;

namespace SentimentAnalysis.API.Controllers;

[ApiController]
[Route("api/keywords")]
public class KeywordsController : ControllerBase
{
    private readonly IReferenceDataService _refService;

    public KeywordsController(IReferenceDataService refService)
        => _refService = refService;

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] string? q = null)
        => Ok(await _refService.SearchKeywordsAsync(q));
}