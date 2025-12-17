using F1Simulator.Models.DTOs.TeamManegementService.TeamDTO;
using F1Simulator.TeamManagementService.Repositories;
using F1Simulator.TeamManagementService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace F1Simulator.TeamManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly TeamService _teamService;
        private readonly ILogger<TeamController> _logger;

        public TeamController(TeamService teamService, ILogger<TeamController> logger)
        {
            _teamService = teamService;
            _logger = logger;
        }

        [HttpGet("{count}")]
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
                _teamService.CreateTeamAsync(teamRequestDto);
                _logger.LogInformation("Team created successfully!");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating team");
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<TeamResponseDTO>>> GetAllTeamsAsync()
        {
            try
            {
                var teams = await _teamService.GetAllTeamsAsync();
                return Ok(teams);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting the team list: {ex.Message}", ex);
                throw;
            }
        }

        [HttpGet("{teamId}")]
        public async Task<TeamResponseDTO> GetTeamByIdAsync(string teamId)
        {

        }
    }
}
