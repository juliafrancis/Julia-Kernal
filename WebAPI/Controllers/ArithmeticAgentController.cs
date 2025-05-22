using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebAPI.Constants;

[ApiController]
[Route("api/arithmetic-agent")]
public class ArithmeticAgentController : ControllerBase
{
    [HttpPost]
    [EndpointDescription(EndpointDescription.arithmetic_agent)]
    public IActionResult Calculate([FromBody] ArithmeticRequest req)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Ảo ma tính toán
        var adjustedBurn = req.InitialSupply * req.BurnRate;
        var inflated = (req.InitialSupply - adjustedBurn) * (1 + req.InflationRate);
        var finalSupply = Math.Round(inflated);

        var comment = req.InflationRate > 0.03
            ? "High inflation risk detected. Rebase advisable."
            : req.InflationRate < 0.005
                ? "Deflationary pressure detected. Supply tightening."
                : "Sustainable inflation detected. Token is mildly expansionary.";

        return Ok(new
        {
            final_supply_projection = finalSupply,
            commentary = comment,
            model = "ArithmeticAgent v0.3"
        });
    }

    public class ArithmeticRequest
    {
        [Required]
        public double InitialSupply { get; set; }

        [Required]
        public double BurnRate { get; set; } // Ex: 0.05 = 5%

        [Required]
        public double InflationRate { get; set; } // Ex: 0.01 = 1%
    }
}