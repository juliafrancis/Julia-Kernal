using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebAPI.Constants;

[ApiController]
[Route("api/zero-knowledge-voting")]
public class ZeroKnowledgeVotingController : ControllerBase
{
    [EndpointDescription(EndpointDescription.zero_knowledge_voting)]
    [HttpPost]
    public IActionResult Vote([FromBody] ZKVoteRequest req)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var rng = new Random(req.Wallet.GetHashCode() ^ DateTime.UtcNow.Millisecond);
        var deanonymRisk = rng.NextDouble() * 0.01;

        var result = new
        {
            vote_id = $"zkvote-{Guid.NewGuid().ToString("N").Substring(0, 8)}",
            proposalId = req.ProposalId,
            vote_cast = req.Vote,
            zk_proof_valid = true,
            anonymity_level = deanonymRisk < 0.001 ? "Maximal" :
                deanonymRisk < 0.005 ? "Ultra" :
                "Medium",
            risk_of_deanonymization = $"{deanonymRisk * 100:F3}%",
            message = "Your vote was cast anonymously. Thank you for protecting the memecracy."
        };

        return Ok(result);
    }
}

public class ZKVoteRequest
{
    [Required]
    public string Wallet { get; set; }

    [Required]
    public string ProposalId { get; set; }

    [Required]
    [RegularExpression("yes|no|abstain", ErrorMessage = "Vote must be 'yes', 'no', or 'abstain'")]
    public string Vote { get; set; }
}