using F1Simulator.Models.DTOs.TeamManegementService.CarDTO;
using F1Simulator.Models.Models.TeamManegement;
using F1Simulator.TeamManagementService.Repositories.Interfaces;
using F1Simulator.TeamManagementService.Services.Interfaces;

namespace F1Simulator.TeamManagementService.Services
{
    public class CarService : ICarService
    {
        private ICarRepository _carRepository;
        private ITeamRepository _teamRepository;
        private readonly Random _random = Random.Shared;

        public CarService(ICarRepository carRepository, ITeamRepository teamRepository)
        {
            _carRepository = carRepository;
            _teamRepository = teamRepository;
        }


        public async Task CreateCarAsync(CarRequestDTO car)
        {
            if (car is null)
                throw new ArgumentException(nameof(car), "Car cannot be null.");

            if (!Guid.TryParse(car.TeamId, out var teamId))
                throw new ArgumentException("TeamId must be a valid GUID.");

            if (car.WeightKg <= 0)
                throw new ArgumentException("WeightKg must be greater than zero.");

            if (car.Speed <= 0)
                throw new ArgumentException("Speed must be greater than zero.");

            var team = await _teamRepository.GetTeamByIdAsync(Guid.Parse(car.TeamId));
            
            if (team is null)
                throw new ArgumentException("Team does not exist for the provided TeamId.");

            var ca = Math.Round((10.0 * _random.NextDouble()), 3);
            var cp = Math.Round((10.0 * _random.NextDouble()), 3);

            var newCar = new Car
            (
                teamId,
                team.NameAcronym,
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


        public async Task UpdateCarModelAsync(CarModelUpdateDTO carModelUpdate, string carId)
        {
            await _carRepository.UpdateCarModelAsync(carModelUpdate, carId);
        }

    }
}
