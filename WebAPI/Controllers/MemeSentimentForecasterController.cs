using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

[ApiController]
[Route("api/meme-sentiment-forecaster")]
public class MemeSentimentForecasterController : ControllerBase
{
    [HttpPost]
    public IActionResult Forecast([FromBody] MemeSentimentRequest req)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var rng = new Random(req.Token.GetHashCode() ^ DateTime.UtcNow.Millisecond);
        var vibe = 50 + rng.NextDouble() * 50;
        var virality = 60 + rng.NextDouble() * 40;

        var bullishness =
            vibe > 90 ? "Extreme" :
            vibe > 70 ? "High" :
            vibe > 50 ? "Moderate" :
            "Low";

        var trendPhrases = new[]
        {
            "Upward momentum detected across memetic clusters.",
            "Bullish memes spreading faster than expected.",
            "Memetic noise spiking. Caution advised.",
            "Sentiment fading across key influencer nodes.",
            "Highly coordinated emoji activity observed."
        };

        var result = new
        {
            token = req.Token,
            timeframe = req.Timeframe,
            vibe_index = Math.Round(vibe, 1),
            bullishness,
            virality_score = Math.Round(virality, 1),
            trend = trendPhrases[rng.Next(trendPhrases.Length)],
            data_sources = new[] { "Twitter", "Farcaster", "Telegram", "Unknown .eth Botnet" }
        };

        return Ok(result);
    }

    public class MemeSentimentRequest
    {
        [Required]
        public string Token { get; set; }

        [Required]
        [RegularExpression("1h|6h|12h|24h|7d", ErrorMessage = "Timeframe must be 1h, 6h, 12h, 24h, or 7d")]
        public string Timeframe { get; set; }
    }
}