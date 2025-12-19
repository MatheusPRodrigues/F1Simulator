using F1Simulator.Models.DTOs.RaceControlService;
using F1Simulator.RaceControlService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace F1Simulator.RaceControlService.Controllers
{
    [Route("api/race")]
    [ApiController]
    public class RaceControlController : ControllerBase
    {
        private readonly ILogger<RaceControlController> _logger;
        private readonly IRaceControlService _raceControlService;

        public RaceControlController
        (
            ILogger<RaceControlController> logger,
            IRaceControlService raceControlService
        )
        {
            _logger = logger;
            _raceControlService = raceControlService;
        }

        [HttpPost("{raceId}/simulate/tl1")]
        public async Task<ActionResult<List<DriverComparisonResponseDTO>>> ExecuteTlOneSectionAsync([FromRoute] string raceId)
        {
            try
            {
                var result = await _raceControlService.ExecuteTlOneSectionAsync(raceId);
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while request 'ExecuteTlOne' endpoint", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred." });
            }
        }

        [HttpPost("{raceId}/simulate/tl2")]
        public async Task<ActionResult<List<DriverComparisonResponseDTO>>> ExecuteTlTwoSectionAsync([FromRoute] string raceId)
        {
            try
            {
                var result = await _raceControlService.ExecuteTlTwoSectionAsync(raceId);
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while request 'ExecuteTlTwo' endpoint", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred." });
            }
        }

        [HttpPost("{raceId}/simulate/tl3")]
        public async Task<ActionResult<List<DriverComparisonResponseDTO>>> ExecuteTlThreeSectionAsync([FromRoute] string raceId)
        {
            try
            {
                var result = await _raceControlService.ExecuteTlThreeSectionAsync(raceId);
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while request 'ExecuteTlThree' endpoint", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred." });
            }
        }

        [HttpPost("{raceId}/simulate/qualifier")]
        public async Task<ActionResult<List<DriverGridResponseDTO>>> ExecuteQualifierSectionAsync([FromRoute] string raceId)
        {
            try
            {
                var result = await _raceControlService.ExecuteQualifierSectionAsync(raceId);
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while request 'ExecuteQualifier' endpoint", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred." });
            }
        }

        [HttpPost("{raceId}/simulate/race")]
        public async Task<ActionResult<List<DriverGridFinalRaceResponseDTO>>> ExecuteRaceSectionAsync([FromRoute] string raceId)
        {
            try
            {
                var result = await _raceControlService.ExecuteRaceSectionAsync(raceId);
                return StatusCode(StatusCodes.Status201Created,  result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while request 'ExecuteRace' endpoint", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred." } );
            }
        }
    }
}
