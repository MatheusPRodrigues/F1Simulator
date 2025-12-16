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
        public async Task<IActionResult> PutCarCoefficientsAsync([FromBody] EngineersPutDTO engId, string carId)
        {
            try
            {
                var result = await _engineeringService.PutCarCoefficientsAsync(engId, carId);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");

                return Problem(ex.Message);
            }
        }



        [HttpPatch("car/{carId}/ca")]
        public async Task<IActionResult> PatchCarAerodynamicCoefficientsAsync(string carId)
        {
            try
            {
                var result = await _engineeringService.PatchCarAerodynamicCoefficientsCoefficientsAsync(carId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");

                return Problem(ex.Message);
            }
        }



        [HttpPatch("car/{carId}/cp")]
        public async Task<IActionResult> PatchCarPotentialCoefficientsAsync(string carId)
        {
            try
            {
                var result = await _engineeringService.PatchCarPotentialCoefficientsAsync(carId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");

                return Problem(ex.Message);
            }
        }




    }
}
