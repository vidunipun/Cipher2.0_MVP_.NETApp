using Microsoft.AspNetCore.Mvc;
using SentimentAnalysis.API.Models;
using SentimentAnalysis.API.Services;

namespace SentimentAnalysis.API.Controllers;

[ApiController]
[Route("api/reviews")]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewsController(IReviewService reviewService)
        => _reviewService = reviewService;

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] string productId)
    {
        if (string.IsNullOrEmpty(productId))
            return BadRequest("productId required");

        var reviews = await _reviewService.GetReviewsByProductAsync(productId);
        return Ok(reviews);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Review dto)
    {
        if (string.IsNullOrEmpty(dto.ProductId))
            return BadRequest("ProductId required");

        var created = await _reviewService.CreateReviewAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id, [FromQuery] string? productId = null)
    {
        var review = await _reviewService.GetReviewByIdAsync(id, productId);
        return review is null ? NotFound() : Ok(review);
    }
}