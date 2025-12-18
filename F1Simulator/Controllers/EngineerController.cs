using F1Simulator.Models.DTOs.TeamManegementService.EngineerDTO;
using F1Simulator.TeamManagementService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
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

            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
