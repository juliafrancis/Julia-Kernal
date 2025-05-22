using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebAPI.Constants;

[ApiController]
[Route("api/timeline-bender")]
public class TimelineBenderController : ControllerBase
{
    [EndpointDescription(EndpointDescription.timeline_bender)]
    [HttpPost]
    public IActionResult Simulate([FromBody] TimelineRequest req)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var rng = new Random(req.Wallet.GetHashCode() ^ req.Token.GetHashCode() ^ DateTime.UtcNow.Millisecond);
        var altProfit = 1000 + rng.NextDouble() * 100000;
        var regretFactor = rng.NextDouble() * 100;

        var advice = regretFactor switch
        {
            < 20 => "Minimal timeline deviation. You’re on track.",
            < 50 => "You could have exited at peak. Alas.",
            < 80 => "Holding longer might’ve been glorious.",
            _ => "Ape responsibly next time, time traveler."
        };

        return Ok(new
        {
            wallet = req.Wallet,
            token = req.Token,
            simulated_profit = $"${altProfit:F2}",
            regret_factor = $"{regretFactor:F1}%",
            timeline_rating = advice,
            simulation_id = $"bend-{Guid.NewGuid().ToString("N").Substring(0, 8)}"
        });
    }

    public class TimelineRequest
    {
        [Required]
        public string Wallet { get; set; }

        [Required]
        public string Token { get; set; }
    }
}