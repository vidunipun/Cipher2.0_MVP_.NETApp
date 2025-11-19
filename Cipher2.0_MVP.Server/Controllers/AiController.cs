using Microsoft.AspNetCore.Mvc;

namespace SentimentAnalysis.API.Controllers
{
    [ApiController]
    [Route("api/ai")]
    public class AiController : ControllerBase
    {
        // POST /api/ai/query { prompt: "..." }
        [HttpPost("query")]
        public IActionResult Query([FromBody] QueryDto dto)
        {
            // Stub: integrate real AI/model here.
            var response = new { text = $"(stub) Received: {dto.Prompt}", sources = Array.Empty<string>() };
            return Ok(response);
        }

        public record QueryDto(string Prompt);
    }
}