using F1Simulator.Models.DTOs.RaceControlService;
using F1Simulator.RaceControlService.Services.Interfaces;
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

        [HttpPost("simulate/tl1")]
        public async Task<ActionResult<List<DriverComparisonResponseDTO>>> ExecuteTlOneSectionAsync()
        {
            try
            {
                var result = await _raceControlService.ExecuteTlOneSectionAsync();
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(409, new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return StatusCode(404, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while request 'ExecuteTlOne' endpoint", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred." });
            }
        }

        [HttpPost("simulate/tl2")]
        public async Task<ActionResult<List<DriverComparisonResponseDTO>>> ExecuteTlTwoSectionAsync()
        {
            try
            {
                var result = await _raceControlService.ExecuteTlTwoSectionAsync();
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(409, new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return StatusCode(404, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while request 'ExecuteTlTwo' endpoint", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred." });
            }
        }

        [HttpPost("simulate/tl3")]
        public async Task<ActionResult<List<DriverComparisonResponseDTO>>> ExecuteTlThreeSectionAsync()
        {
            try
            {
                var result = await _raceControlService.ExecuteTlThreeSectionAsync();
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(409, new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return StatusCode(404, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while request 'ExecuteTlThree' endpoint", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred." });
            }
        }

        [HttpPost("simulate/qualifier")]
        public async Task<ActionResult<List<DriverGridResponseDTO>>> ExecuteQualifierSectionAsync()
        {
            try
            {
                var result = await _raceControlService.ExecuteQualifierSectionAsync();
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(409, new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return StatusCode(404, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while request 'ExecuteQualifier' endpoint", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred." });
            }
        }

        [HttpPost("simulate/race")]
        public async Task<ActionResult<List<DriverGridFinalRaceResponseDTO>>> ExecuteRaceSectionAsync()
        {
            try
            {
                var result = await _raceControlService.ExecuteRaceSectionAsync();
                return StatusCode(StatusCodes.Status201Created,  result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(409, new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return StatusCode(404, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while request 'ExecuteRace' endpoint", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred." } );
            }
        }

        [HttpGet("{raceId}")]
        public async Task<ActionResult<RaceControlResponseDTO>> GetRaceByRaceIdAsync(Guid raceId)
        {
            try
            {
                var response = await _raceControlService.GetRaceByRaceIdAsync(raceId);
                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (KeyNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { Message =  ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while request 'Get By RaceId' endpoint", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred." });
            }
        }

        [HttpGet("season/{seasonYear}")]
        public async Task<ActionResult<RaceControlResponseDTO>> GetRacesBySeasonYearAsync(int seasonYear)
        {
            try
            {
                var response = await _raceControlService.GetRacesBySeasonYearAsync(seasonYear);
                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (KeyNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while request 'Get By Season Year' endpoint", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred." });
            }
        }
    }
}
