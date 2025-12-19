using F1Simulator.Models.DTOs.TeamManegementService.BossDTO;
using F1Simulator.Models.DTOs.TeamManegementService.TeamDTO;
using F1Simulator.Models.Models.TeamManegement;
using F1Simulator.Models.Models.TeamManegementService;
using F1Simulator.TeamManagementService.Services;
using F1Simulator.TeamManagementService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization.Serializers;

namespace F1Simulator.TeamManagementService.Controllers
{
    [Route("api/boss")]
    [ApiController]
    public class BossController : ControllerBase
    {
        private readonly IBossService _bossService;
        private readonly ILogger<BossController> _logger;

        public BossController(IBossService bossService, ILogger<BossController> logger)
        {
            _bossService = bossService;
            _logger = logger;
        }

        [HttpGet("count/{teamId}")]
        public async Task<ActionResult<int>> GetBossByTeamCountAsync(string teamId)
        {
            try
            {
                _logger.LogInformation("Count of teams:");
                return await _bossService.GetBossByTeamCountAsync(teamId);
            }
            catch(Exception ex)
            {

                _logger.LogError(ex, "Error occurred while getting count of bosess in team!");
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateBossAsync([FromBody] BossRequestDTO bossDto) 
        {
            try
            {
               await _bossService.CreateBossAsync(bossDto);
                _logger.LogInformation("Team sucessfully created!");
                return StatusCode(StatusCodes.Status201Created); ;
            }
            catch(ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating boss!");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unspected error ocurred while creating boss!!" });
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<BossResponseDTO>>> GetAllBossesAsync()
        {
            try
            {
                var boss = await _bossService.GetAllBossesAsync();

                if (boss is null || boss.Count == 0)
                    return NoContent();

                return Ok(boss);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all bosses!");
                throw;
            }
        }

        [HttpGet("boss-team")]
        public async Task<ActionResult<List<BossWithTeamDTO>>> GetBossesWithTeamAsync()
        {
            try
            {
                var bossTeam = await _bossService.GetBossesWithTeamAsync();
                return bossTeam is null ? NotFound() : Ok(bossTeam);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all bosses with their teams!");
                throw;
            }
        }
    }
}
