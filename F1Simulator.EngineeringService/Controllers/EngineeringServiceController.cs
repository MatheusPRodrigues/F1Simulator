using F1Simulator.EngineeringService.Services.Interfaces;
using F1Simulator.Models.DTOs.EngineeringService;
using Microsoft.AspNetCore.Mvc;

namespace F1Simulator.EngineeringService.Controllers
{
    [Route("api/engineering")]
    [ApiController]
    public class EngineeringServiceController : ControllerBase
    {
        private readonly ILogger<EngineeringServiceController> _logger;
        private readonly IEngineeringService _engineeringService;

        public EngineeringServiceController(ILogger<EngineeringServiceController> logger, IEngineeringService engineeringService)
        {
            _logger = logger;
            _engineeringService = engineeringService;
        }



        [HttpPut("car/{carId}")]
        public async Task<ActionResult> PutCarCoefficientsAsync([FromBody] EngineersPutDTO engIds, [FromRoute] string carId)
        {
            try
            {
                var result = await _engineeringService.PutCarCoefficientsAsync(engIds, carId);

                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (KeyNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");

                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message});
            }
        }

    }
}
