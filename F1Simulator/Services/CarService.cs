using F1Simulator.Models.DTOs.TeamManegementService.CarDTO;
using F1Simulator.Models.Models.TeamManegement;
using F1Simulator.TeamManagementService.Repositories.Interfaces;
using F1Simulator.TeamManagementService.Services.Interfaces;
using F1Simulator.Utils.Clients.Interfaces;

namespace F1Simulator.TeamManagementService.Services
{
    public class CarService : ICarService
    {
        private readonly ILogger<CarService> _logger;
        private readonly ICarRepository _carRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly Random _random = Random.Shared;
        private readonly ICompetitionClient _competitionClient;

        public CarService(ICarRepository carRepository, ITeamRepository teamRepository, ICompetitionClient competitionClient)
        {
            _carRepository = carRepository;
            _teamRepository = teamRepository;
            _competitionClient = competitionClient;
        }


        public async Task CreateCarAsync(CarRequestDTO car)
        {
            try
            {
                var activeSeason = await _competitionClient.GetActiveSeasonAsync();

                if (activeSeason is not null && activeSeason.IsActive)
                    throw new InvalidOperationException("Cannot create or update cars while a competition season is active.");

                if (car is null)
                    throw new ArgumentException(nameof(car), "Car cannot be null.");

                if (!Guid.TryParse(car.TeamId, out var teamId))
                    throw new ArgumentException("TeamId must be a valid GUID.");

                var countCarsTeam = await _carRepository.GetCountCarsByIdTeamAsync(car.TeamId);

                if (countCarsTeam >= 2)
                    throw new ArgumentException("The car cannot be added because there are already 2 cars registered in the team.");

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
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while creating the car.: {ex.Message}", ex);
                throw;
            }
        }


        public async Task<List<CarResponseDTO>> GetAllCarAsync()
        {
            try
            {
                return await _carRepository.GetAllCarAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while listing the cars: {ex.Message}", ex);
                throw;
            }
        }


        public async Task<CarResponseDTO> GetCarByIdAsync(string id)
        {
            try
            {
                var car = await _carRepository.GetCarByIdAsync(id);

                if (car is null)
                    throw new KeyNotFoundException("Car not found.");

                return car;
            }
            catch (Exception ex)
            {
                _logger.LogError($"There was an error while searching for the car: {ex.Message}", ex);
                throw;
            }
        }


        public async Task UpdateCarCoefficientsAsync(CarUpdateDTO carUpdate, string carId)
        {
            try
            {
                await _carRepository.UpdateCarCoefficientsAsync(carUpdate, carId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating the car's coefficients: {ex.Message}", ex);
                throw;
            }
        }


        public async Task UpdateCarModelAsync(CarModelUpdateDTO carModelUpdate, string carId)
        {
            try
            {
                var activeSeason = await _competitionClient.GetActiveSeasonAsync();

                if (activeSeason is not null && activeSeason.IsActive)
                    throw new InvalidOperationException("Cannot create or update cars while a competition season is active.");

                await _carRepository.UpdateCarModelAsync(carModelUpdate, carId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating the car model: {ex.Message}", ex);
                throw;
            }
        }


        public async Task<int> GetCountCarByIdCarAsync(Guid carId)
        {
            try
            {
                return await _carRepository.GetCountCarsByIdCarAsync(carId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"There was an error while counting cars per driver: {ex.Message}", ex);
                throw;
            }
        }
    }
}
