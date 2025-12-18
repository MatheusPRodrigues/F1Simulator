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

                return Created();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating a new car.");

                return Problem(ex.Message);
            }
        }


        [HttpGet]
        public async Task<ActionResult<List<CarResponseDTO>>> GetAllCarAsync()
        {
            try
            {
                var cars = await _carService.GetAllCarAsync();

                if (cars.IsNullOrEmpty())
                    return NoContent();

                return Ok(cars);
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
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while searching for the car.");

                return Problem(ex.Message);
            }
        }


        [HttpPut("{carId}/coefficients")]
        public async Task<IActionResult> UpdateCarCoefficientsAsync([FromBody] CarUpdateDTO carUpdate, [FromRoute] string carId)
        {
            try
            {
                await _carService.UpdateCarCoefficientsAsync(carUpdate, carId);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating the coefficients.");

                return Problem(ex.Message);
            }
        }


        [HttpPatch("{carId}/model")]
        public async Task<IActionResult> UpdateCarModelAsync([FromBody] CarModelUpdateDTO carModelUpdate, [FromRoute] string carId)
        {
            try
            {
                await _carService.UpdateCarModelAsync(carModelUpdate, carId);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating the model.");

                return Problem(ex.Message);
            }
        }


    }
}
