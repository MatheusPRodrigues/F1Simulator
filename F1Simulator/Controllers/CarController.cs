using F1Simulator.Models.DTOs.TeamManegementService.CarDTO;
using F1Simulator.TeamManagementService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace F1Simulator.TeamManagementService.Controllers
{
    [Route("api/car")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ILogger<CarController> _logger;
        private readonly ICarService _carService;

        public CarController(ILogger<CarController> logger, ICarService carService)
        {
            _logger = logger;
            _carService = carService;
        }


        [HttpPost]
        public async Task<ActionResult> CreateCarAsync([FromBody] CarRequestDTO car)
        {
            try
            {
                await _carService.CreateCarAsync(car);

                return StatusCode(StatusCodes.Status201Created, car);
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

                _logger.LogError(ex, "Error occurred while creating the car!");
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<CarResponseDTO>>> GetAllCarAsync()
        {
            try
            {
                var cars = await _carService.GetAllCarAsync();

                if (cars.IsNullOrEmpty() || cars.Count == 0)
                    return NoContent();

                return Ok(cars);
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
                _logger.LogError(ex, "An unexpected error occurred while listing the cars.");

                return Problem(ex.Message);
            }
        }


        [HttpGet("{carId}")]
        public async Task<ActionResult<CarResponseDTO>> GetCarByIdAsync([FromRoute] string carId)
        {
            try
            {
                var car = await _carService.GetCarByIdAsync(carId);

                return Ok(car);
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
                _logger.LogError(ex, "An unexpected error occurred while searching for the car.");

                return Problem(ex.Message);
            }
        }


        [HttpPut("{carId}/coefficients")]
        public async Task<ActionResult> UpdateCarCoefficientsAsync([FromBody] CarUpdateDTO carUpdate, [FromRoute] string carId)
        {
            try
            {
                await _carService.UpdateCarCoefficientsAsync(carUpdate, carId);

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
                _logger.LogError(ex, "An unexpected error occurred while updating the coefficients.");

                return Problem(ex.Message);
            }
        }


        [HttpPatch("{carId}/model")]
        public async Task<ActionResult> UpdateCarModelAsync([FromBody] CarModelUpdateDTO carModelUpdate, [FromRoute] string carId)
        {
            try
            {
                await _carService.UpdateCarModelAsync(carModelUpdate, carId);

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
                _logger.LogError(ex, "An unexpected error occurred while updating the model.");

                return Problem(ex.Message);
            }
        }
    }
}
