using F1Simulator.Models.DTOs.TeamManegementService.CarDTO;
using F1Simulator.Models.Models.TeamManegement;
using F1Simulator.TeamManagementService.Repositories.Interfaces;
using F1Simulator.TeamManagementService.Services.Interfaces;

namespace F1Simulator.TeamManagementService.Services
{
    public class CarService : ICarService
    {
        private ICarRepository _carRepository;
        private readonly Random _random = Random.Shared;

        public CarService(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }


        public async Task CreateCarAsync(CarRequestDTO car)
        {
            if (car is null)
                throw new ArgumentException(nameof(car), "Car cannot be null.");

            if (!Guid.TryParse(car.TeamId, out var teamId))
                throw new ArgumentException("TeamId must be a valid GUID.");

            if (car.Model.Length != 5) 
                throw new ArgumentException("Model length must be exactly 5 characters.");

            if (car.WeightKg <= 0)
                throw new ArgumentException("WeightKg must be greater than zero.");

            if (car.Speed <= 0)
                throw new ArgumentException("Speed must be greater than zero.");

            var team = await _carRepository.GetTeamByIdAsync(car.TeamId);
            
            if (team is null)
                throw new ArgumentException("Team does not exist for the provided TeamId.");

            if (!car.Model.Substring(0, 3).Equals(team.NameAcronym, StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("The first three characters of the car model must match the team's acronym.");

            if (!char.IsDigit(car.Model[3]) || !char.IsDigit(car.Model[4]))
                throw new ArgumentException("The last two characters of the car model must be numeric.");


            var ca = Math.Round((10.0 * _random.NextDouble()), 3);
            var cp = Math.Round((10.0 * _random.NextDouble()), 3);

            var newCar = new Car
            (
                teamId,
                car.Model,
                car.WeightKg,
                car.Speed,
                ca,
                cp
            );

            await _carRepository.CreateCarAsync(newCar);
        }


        public async Task<List<CarResponseDTO>> GetAllCarAsync(string id)
        {
            return await _carRepository.GetAllCarAsync(id);
        }


        public async Task<CarResponseDTO> GetCarByIdAsync(string id)
        {
            var car = await _carRepository.GetCarByIdAsync(id);

            if (car is null)
                throw new KeyNotFoundException("Car not found.");

            return car;
        }


        public async Task UpdateCarCoefficientsAsync(CarUpdateDTO carUpdate, string carId)
        {
            await _carRepository.UpdateCarCoefficientsAsync(carUpdate, carId);
        }

    }
}
