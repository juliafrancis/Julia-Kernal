using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebAPI.Constants;

[ApiController]
[Route("api/ai-shiller-rating")]
public class AiShillerRatingController : ControllerBase
{
    [EndpointDescription(EndpointDescription.ai_shiller_rating)]
    [HttpPost]
    public IActionResult Rate([FromBody] ShillRequest req)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var rng = new Random(req.Content.Length ^ DateTime.UtcNow.Millisecond);
        var engagement = 60 + rng.NextDouble() * 40;
        var hype = 40 + rng.NextDouble() * 60;
        var rugAvoidance = rng.NextDouble() * 100;

        return Ok(new
        {
            content = req.Content,
            engagement_potential = $"{engagement:F1}%",
            hype_density = $"{hype:F1}%",
            rug_avoidance_index = $"{rugAvoidance:F2}%",
            overall_score = $"{((engagement + hype + rugAvoidance) / 3):F1}%",
            verdict = engagement + hype > 150 ? "Certified Shillmaster" : "Needs More Vibes",
            evaluated_by = "ShillGPT v4.20"
        });
    }

    public class ShillRequest
    {
        [Required]
        public string Content { get; set; }
    }
}