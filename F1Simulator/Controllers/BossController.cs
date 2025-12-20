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
                _logger.LogInformation("Count of boss in the team:");
                return await _bossService.GetBossByTeamCountAsync(teamId);
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
                _logger.LogError(ex, "Error occurred while getting the bosses!");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unspected error ocurred while getting the bosses!" });
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
                _logger.LogError(ex, "Error occurred while getting all bosses with their teams!");
                throw;
            }
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetAllBossesCountAsync()
        {
            try
            {
                _logger.LogInformation("Count of all boss:");
                return await _bossService.GetAllBossesCountAsync();
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

                _logger.LogError(ex, "Error occurred while getting count all bosess!");
                return BadRequest(ex);
            }
        }
    }
}
