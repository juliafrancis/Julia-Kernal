using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebAPI.Constants;

[ApiController]
[Route("api/reinforcement-learning")]
public class ReinforcementLearningController : ControllerBase
{
    [HttpPost]
    [EndpointDescription(EndpointDescription.reinforcement_learning_plugin)]
    public IActionResult Learn([FromBody] ReinforcementLearningRequest req)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var rng = new Random(req.Token.GetHashCode() ^ DateTime.UtcNow.Millisecond);
        var reward = 5 + rng.NextDouble() * 20;
        var confidence = 70 + rng.NextDouble() * 25;

        string strategy = req.Mode switch
        {
            "aggressive" => "Buy small dips and stack aggressively every 3.5h",
            "balanced" => "Buy on retracements and take partial profits at 8%",
            "conservative" => "Wait for confirmations, enter only during low volatility",
            _ => "Unknown strategy"
        };

        var result = new
        {
            token = req.Token,
            mode = req.Mode,
            strategy,
            expected_reward = $"{reward:F2}%",
            confidence = $"{confidence:F2}%",
            model_id = $"rl-{Guid.NewGuid().ToString("N").Substring(0, 8)}"
        };

        return Ok(result);
    }
}

public class ReinforcementLearningRequest
{
    [Required]
    public string Token { get; set; }

    [Required]
    public string Wallet { get; set; }

    [Required]
    [RegularExpression("aggressive|balanced|conservative", ErrorMessage = "Mode must be aggressive, balanced, or conservative")]
    public string Mode { get; set; }
}