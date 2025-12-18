using F1Simulator.Models.DTOs.TeamManegementService.EngineerDTO;
using F1Simulator.TeamManagementService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace F1Simulator.TeamManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EngineerController : ControllerBase
    {
        private readonly IEngineerService _engineerService;

        public EngineerController(IEngineerService engineerService)
        {
            _engineerService = engineerService;
        }

        [HttpPost]
        public async Task<ActionResult<EngineerResponseDTO>> CreateEngineerAsync([FromBody] EngineerRequestDTO engineerRequestDTO)
        {
            try
            {
                var engineer = await _engineerService.CreateEngineerAsync(engineerRequestDTO);
                return Ok(engineer);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<EngineerResponseDTO>>> GetAllEngineersAsync()
        {
            try
            {
                var enginners = await _engineerService.GetAllEngineersAsync();
                if (enginners is null)
                    return NotFound();
                return Ok(enginners);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<EngineerResponseDTO>> GetEngineerByIdAsync([FromRoute] Guid id)
        {
            try
            {
                var engineer = await _engineerService.GetEngineerByIdAsync(id);
                if (engineer is null)
                    return NotFound();
                return Ok(engineer);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateEnginnerAsync([FromBody] EngineerUpdateRequestDTO engineerUpdateRequestDTO, [FromRoute] Guid id)
        {
            try
            {
                await _engineerService.UpdateActiveEngineer(engineerUpdateRequestDTO, id);
                return NoContent();
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
