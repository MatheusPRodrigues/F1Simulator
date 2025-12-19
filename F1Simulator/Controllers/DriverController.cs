using F1Simulator.Models.DTOs.RaceControlService;
using F1Simulator.Models.DTOs.TeamManegementService.DriverDTO;
using F1Simulator.TeamManagementService.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace F1Simulator.TeamManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IDriverService _driverService;
        public DriverController(IDriverService driverService)
        {
            _driverService = driverService;
        }
        [HttpPost]
        public async Task<ActionResult<DriverResponseDTO>> CreateDriverAsync([FromBody] DriverRequestDTO driverRequestDTO)
        {
            try
            {
                var driver = await _driverService.CreateDriverAsync(driverRequestDTO);
                return StatusCode(201, driver);
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
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<DriverResponseDTO>> GetAllDriversAsync()
        {
            try
            {
                var drivers = await _driverService.GetDriversAsync();
                if (drivers is null || drivers.Count == 0)
                    return NoContent();
                return Ok(drivers);
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
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("race")]
        public async Task<ActionResult<DriverToRaceDTO>> GetAllDriversToRaceAsync()
        {
            try
            {
                var drivers = await _driverService.GetDriversToRaceAsync();
                if (drivers is null || drivers.Count == 0)
                    return NoContent();
                return Ok(drivers);
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
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DriverResponseDTO>> GetDriverByIdAsync([FromRoute] Guid id)
        {
            try
            {
                var driver = await _driverService.GetDriverByIdAsync(id);
                if (driver is null)
                    return NoContent();
                return Ok(driver);
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
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<DriverResponseDTO>> UpdateDriverAsync([FromBody] UpdateRequestDriverDTO driverRequestDTO, [FromRoute] Guid id)
        {
            try
            {
                await _driverService.UpdateDriverAsync(id, driverRequestDTO);
                return NoContent();
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
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
