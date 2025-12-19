using F1Simulator.Models.DTOs.TeamManegementService.TeamDTO;
using F1Simulator.Models.Models.TeamManegement;
using F1Simulator.TeamManagementService.Repositories;
using F1Simulator.TeamManagementService.Services;
using F1Simulator.TeamManagementService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace F1Simulator.TeamManagementService.Controllers
{
    [Route("api/team")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;
        private readonly ILogger<TeamController> _logger;

        public TeamController(ITeamService teamService, ILogger<TeamController> logger)
        {
            _teamService = teamService;
            _logger = logger;
        }

        [HttpGet("count")]
        public async Task<int> GetTeamsCountAsync()
        {
            try
            {
                _logger.LogInformation("Count of teams:");
                return await _teamService.GetTeamsCountAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting count the team: {ex.Message}", ex);
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateTeamAsync([FromBody] TeamRequestDTO teamRequestDto)
        {
            try
            {
                await _teamService.CreateTeamAsync(teamRequestDto);
                _logger.LogInformation("Team sucessfully created!");
                return Created();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating team");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<TeamResponseDTO>>> GetAllTeamsAsync()
        {
            try
            {
                var teams = await _teamService.GetAllTeamsAsync();

                return teams is null ? NotFound() : Ok(teams);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting the team list: {ex.Message}", ex);
                throw;
            }
        }

        [HttpGet("{teamId}")]
        public async Task<ActionResult<TeamResponseDTO>> GetTeamByIdAsync([FromRoute] string teamId)
        {
            try
            {
                var result = await _teamService.GetTeamByIdAsync(teamId);
                return result is null ? NotFound() : Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError($"An error occurred while getting the team by id: {ex.Message}", ex);
                throw;
            }
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult<TeamResponseDTO>> GetTeamByNameAsync([FromRoute] string name)
        {
            try
            {
                var result = await _teamService.GetTeamByNameAsync(name);
                return result is null ? NotFound() : Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting the team by name: {ex.Message}", ex);
                throw;
            }
        }

        [HttpPut("update/{teamId}")]
        public async Task<ActionResult> UpdateTeamCountryAsync([FromRoute]string teamId, [FromBody] string newCountry)
        {
            try
            {
                await _teamService.UpdateTeamCountryAsync(teamId, newCountry);

                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError($"An error occurred while updatting team: {ex.Message}", ex);
                throw;
            }
        }
    }
}
