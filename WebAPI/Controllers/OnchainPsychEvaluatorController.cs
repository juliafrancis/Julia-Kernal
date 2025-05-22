using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebAPI.Constants;

[ApiController]
[Route("api/onchain-psych-evaluator")]
public class OnchainPsychEvaluatorController : ControllerBase
{
    [HttpPost]
    [EndpointDescription(EndpointDescription.onchain_psych_evaluator)]
    public IActionResult Evaluate([FromBody] PsychEvalRequest req)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var rng = new Random(req.Wallet.GetHashCode() ^ DateTime.UtcNow.Millisecond);
        var score = rng.NextDouble() * 100;

        var archetypes = new[]
        {
            "FOMO Chaser",
            "Exit Scammer in Denial",
            "Diamond-Handed Monk",
            "Gas Waster",
            "Wallet Splitter",
            "Stablecoin Hermit"
        };

        var mentalRisk = score switch
        {
            < 30 => "High Risk",
            < 60 => "Moderate Risk",
            _ => "Disciplined"
        };

        return Ok(new
        {
            wallet = req.Wallet,
            archetype = archetypes[rng.Next(archetypes.Length)],
            discipline_score = Math.Round(score, 2),
            mental_risk_factor = mentalRisk,
            evaluated_by = "PsychChain™ v1.2"
        });
    }

    public class PsychEvalRequest
    {
        [Required]
        public string Wallet { get; set; }
    }
}