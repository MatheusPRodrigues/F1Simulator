using F1Simulator.CompetitionService.Exceptions;
using F1Simulator.CompetitionService.Services.Interfaces;
using F1Simulator.Models.DTOs.CompetitionService.Response;
using Microsoft.AspNetCore.Mvc;

namespace F1Simulator.CompetitionService.Controllers
{
    [Route("api/competition")]
    [ApiController]
    public class CompetitionController : ControllerBase
    {

        private readonly ILogger<CompetitionController> _logger;
        private readonly ICompetitionService _competitionService;
        public CompetitionController(ILogger<CompetitionController> logger, ICompetitionService competitionService)
        {
            _logger = logger;
            _competitionService = competitionService;
        }

        [HttpGet("season/active")]
        public async Task<ActionResult> GetCompetitionActiveAsync()
        {
            try
            {
                var competition = await _competitionService.GetCompetitionActiveAsync();
                if (competition == null)
                {
                    return NotFound("There is no season that has started.");
                }
                return Ok(competition);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving competitions");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving competitions.");
            }

        }

        [HttpPost("season/start")]
        public async Task<ActionResult> StartSeasonAsync()
        {
            try
            {
                var result = await _competitionService.StartSeasonAsync();
                return Ok(result);
            }
            catch (BusinessException bex)
            {
                _logger.LogWarning(bex, "Business error starting season");
                return BadRequest(bex.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting season");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while starting the season.");
            }
        }

        [HttpPatch("races/round:{round}/start")]

        public async Task<ActionResult> StartRaceAsync([FromRoute] int round)
        {
            try
            {
                await _competitionService.StartRaceAsync(round);
                return Ok();
            }
            catch (BusinessException bex)
            {
                _logger.LogWarning(bex, "Business error starting");
                return BadRequest(bex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting race for round {RoundId}", round);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while starting the race.");
            }
        }

        [HttpGet("races/inprogress")]
        public async Task<ActionResult> GetRaceInProgressAsync()
        {
            try
            {
                var race = await _competitionService.GetRaceWithCircuitAsync();
                if (race == null)
                {
                    return NotFound("There isn't a race in progress in the current season.");
                }

                return Ok(race);

            }
            catch (BusinessException bex)
            {
                _logger.LogError(bex, "An error occurred while get the race");
                return NotFound(bex.Message);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error get race in progress");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while get the race.");
            }
        }

        [HttpPatch("races/t1")]

        public async Task<ActionResult> UpdateRaceT1Async()
        {
            try
            {
                await _competitionService.UpdateRaceT1Async();
                return Ok();

            }
            catch (BusinessException bex)
            {
                _logger.LogError(bex, "An error occurred while updating T1");
                return NotFound(bex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating T1");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating T1.");
            }
        }

        [HttpPatch("races/t2")]

        public async Task<ActionResult> UpdateT2Async()
        {
            try
            {
                await _competitionService.UpdateRaceT2Async();
                return Ok();

            }
            catch (BusinessException bex)
            {
                _logger.LogError(bex, "An error occurred while updating T2");
                return NotFound(bex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating T2");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating T2.");
            }
        }

        [HttpPatch("races/t3")]

        public async Task<ActionResult> UpdateT3Async()
        {
            try
            {
                await _competitionService.UpdateRaceT3Async();
                return Ok();

            }
            catch (BusinessException bex)
            {
                _logger.LogError(bex, "An error occurred while updating T3");
                return NotFound(bex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating T3");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating T3.");
            }
        }

        [HttpPatch("races/qualifier")]

        public async Task<ActionResult> UpdateQualifierAsync()
        {
            try
            {
                await _competitionService.UpdateRaceQualifierAsync();
                return Ok();

            }
            catch (BusinessException bex)
            {
                _logger.LogError(bex, "An error occurred while updating Qualifier");
                return NotFound(bex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Qualifier");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating Qualifier.");
            }
        }

        [HttpPatch("races/race")]

        public async Task<ActionResult> UpdateRaceRaceAsync()
        {
            try
            {
                await _competitionService.UpdateRaceRaceAsync();
                return Ok();

            }
            catch (BusinessException bex)
            {
                _logger.LogError(bex, "An error occurred while updating race");
                return NotFound(bex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating race");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating race.");
            }
        }

        [HttpGet("driverstanding")]
        public async Task<ActionResult> GetDriverStandingAsync()
        {
            try
            {
                var driverStandings = await _competitionService.GetDriverStandingAsync();
                return Ok(driverStandings);
            }
            catch (BusinessException bex)
            {
                _logger.LogError(bex, "An error occurred while retrieving driver standings");
                return NotFound(bex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving driver standings");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving driver standings.");
            }
        }

        [HttpGet("teamstanding")]
        public async Task<ActionResult> GetTeamStandingAsync()
        {
            try
            {
                var teamStandings = await _competitionService.GetTeamStandingAsync();
                return Ok(teamStandings);
            }
            catch (BusinessException bex)
            {
                _logger.LogError(bex, "An error occurred while retrieving team standings");
                return NotFound(bex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving team standings");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving team standings.");
            }
        }
 
        [HttpGet("calendar")]
        public async Task<ActionResult> GetTeamRacesAsync()
        {
            try
            {
                var calendar = await _competitionService.GetRacesAsync();
                return Ok(calendar);
            }
            catch (BusinessException bex)
            {
                _logger.LogError(bex, "An error occurred while retrieving calendar");
                return NotFound(bex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving calendar");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving calendar.");
            }
        }

        [HttpPost("endrace")]
        public async Task<ActionResult> EndRaceAsync()
        {
            try
            {
                await _competitionService.EndRaceAsync();
                return Ok();
            }
            catch (BusinessException bex)
            {
                _logger.LogWarning(bex, "Business error ending race ");
                return NotFound(bex.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ending race ");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while ending the race.");
            }
        }

        [HttpPost("endseason")]

        public async Task<ActionResult<StandingsResponseDTO>> EndSeasonAsync()
        {
            try
            {
                var standings = await _competitionService.EndSeasonAsync();
                return Ok(standings);
            }
            catch (BusinessException bex)
            {
                _logger.LogWarning(bex, "Business error ending season ");
                return NotFound(bex.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ending race ");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while ending the season.");
            }
        }

        [HttpGet("seasons")]
        public async Task<ActionResult<List<SeasonResponseDTO>>> GetAllSeasonsAsync()
        {
            try
            {
                var seasons = await _competitionService.GetAllSeasonsAsync();
                return Ok(seasons);
            }
            catch (BusinessException bex)
            {
                _logger.LogError(bex, "An error occurred while get seasons");
                return NotFound(bex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving seasons");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving seasons.");
            }
        }
    }
}
