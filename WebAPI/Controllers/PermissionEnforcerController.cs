using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebAPI.Constants;

[ApiController]
[Route("api/permission-enforcer")]
public class PermissionEnforcerController : ControllerBase
{
    [HttpPost]
    [EndpointDescription(EndpointDescription.permission_enforcer)]
    public IActionResult Enforce([FromBody] PermissionRequest req)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var rng = new Random(req.Wallet.GetHashCode() ^ req.Action.Length ^ DateTime.UtcNow.Millisecond);
        var score = 60 + rng.NextDouble() * 40;
        var granted = score > 75;

        var reasonBank = new[]
        {
            "Wallet is whitelisted through ancestral delegation.",
            "Previous DAO interactions suggest authority alignment.",
            "Wallet has historic engagement with meme-tier assets.",
            "Access inferred via probabilistic retroactive airdrop signals.",
            "Permission inferred through non-deterministic token entropy."
        };

        var result = new
        {
            wallet = req.Wallet,
            resource = req.Resource,
            action = req.Action,
            access_granted = granted,
            decision_reason = reasonBank[rng.Next(reasonBank.Length)],
            enforced_by = "ZK-AI Matrix",
            permission_score = Math.Round(score, 2)
        };

        return Ok(result);
    }
}

public class PermissionRequest
{
    [Required]
    public string Wallet { get; set; }

    [Required]
    public string Resource { get; set; }

    [Required]
    public string Action { get; set; }
}
