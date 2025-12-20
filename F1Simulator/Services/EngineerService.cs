using F1Simulator.Models.DTOs.TeamManegementService.EngineerDTO;
using F1Simulator.Models.Models.TeamManegement;
using F1Simulator.TeamManagementService.Repositories.Interfaces;
using F1Simulator.TeamManagementService.Services.Interfaces;
using F1Simulator.Utils.Clients.Interfaces;

namespace F1Simulator.TeamManagementService.Services
{
    public class EngineerService : IEngineerService
    {
        private readonly Random _random = Random.Shared;
        private readonly IEngineerRepository _engineerRepository;
        private readonly ITeamService _teamService;
        private readonly ICarService _carService;
        private readonly ICompetitionClient _competitionClient;

        public EngineerService(IEngineerRepository engineerRepository, ITeamService teamService,
                               ICarService carService, ICompetitionClient competitionClient)
        {
            _engineerRepository = engineerRepository;
            _teamService = teamService;
            _carService = carService;
            _competitionClient = competitionClient;
        }

        public async Task<EngineerResponseDTO> CreateEngineerAsync(EngineerRequestDTO engineerRequestDTO)
        {
            try
            {
                var activeSeason = await _competitionClient.GetActiveSeasonAsync();

                if (activeSeason is not null && activeSeason.IsActive)
                    throw new InvalidOperationException("Cannot create or update engineer while a competition season is active.");

                if (await _engineerRepository.GetAllEngineersCountAsync() >= 44)
                    throw new InvalidOperationException("Maximum number of engineers (44) reached.");

                var team = await _teamService.GetTeamByIdAsync(engineerRequestDTO.TeamId.ToString());

                if (team is null)
                    throw new KeyNotFoundException("The team not found!");

                var car = await _carService.GetCarByIdAsync(engineerRequestDTO.CarId.ToString());

                if (car is null)
                    throw new KeyNotFoundException("The car not found!");

                var engineersInCar = await _engineerRepository.GetEngineersByCarIdAsync(engineerRequestDTO.CarId);

                if (engineersInCar.Count >= 2)
                    throw new InvalidOperationException("This car already has two engineers.");

                if (engineersInCar.Any(e => e.EngineerSpecialization == engineerRequestDTO.EngineerSpecialization))
                    throw new InvalidOperationException("This car already has an engineer with this specialization.");


                var experienceEngineer = Math.Round((10.0 * _random.NextDouble()), 3);

                var engineer = new Engineer(
                    engineerRequestDTO.TeamId,
                    engineerRequestDTO.CarId,
                    engineerRequestDTO.FirstName,
                    engineerRequestDTO.FullName,
                    engineerRequestDTO.EngineerSpecialization,
                    experienceEngineer
                    );

                return await _engineerRepository.CreateEngineerAsync(engineer);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<EngineerResponseDTO>> GetAllEngineersAsync()
        {
            try
            {
                return await _engineerRepository.GetAllEnginnersAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EngineerResponseDTO> GetEngineerByIdAsync(Guid id)
        {
            try
            {
                return await _engineerRepository.GetEngineerByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> GetAllEngineersCountAsync()
        {
            try
            {
                return await _engineerRepository.GetAllEngineersCountAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
