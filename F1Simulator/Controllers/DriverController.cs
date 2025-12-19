using F1Simulator.Models.DTOs.TeamManegementService.DriverDTO;
using Microsoft.AspNetCore.Mvc;

namespace F1Simulator.TeamManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<DriverResponseDTO>> CreateDriverAsync([FromBody] DriverRequestDTO driverRequestDTO)
        {
            try
            {
                return Ok();
            } catch(Exception ex)
            {
                throw new Exception(ex.Message); 
            }
        }

        [HttpGet]
        public async Task<ActionResult<DriverResponseDTO>> GetAllDriversAsync()
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DriverResponseDTO>> GetDriverByIdAsync([FromRoute] Guid id)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ActionResult<DriverResponseDTO>> UpdateDriverAsync([FromBody] DriverRequestDTO driverRequestDTO)
        {
            try
            {
                return NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
