using F1Simulator.Models.DTOs.TeamManegementService.EngineerDTO;
using F1Simulator.Models.DTOs.TeamManegementService.TeamDTO;
using F1Simulator.Models.Models.TeamManegement;
using F1Simulator.TeamManagementService.Repositories.Interfaces;
using F1Simulator.TeamManagementService.Services.Interfaces;

namespace F1Simulator.TeamManagementService.Services
{
    public class EngineerService : IEngineerService
    {
        private readonly Random _random = Random.Shared;
        private readonly IEngineerRepository _engineerRepository;
        private readonly IHttpClientFactory _httpClient;
        public EngineerService(IEngineerRepository engineerRepository, IHttpClientFactory httpClient)
        {
            _engineerRepository = engineerRepository;
            _httpClient = httpClient;
        }

        public async Task<EngineerResponseDTO> CreateEngineerAsync(EngineerRequestDTO engineerRequestDTO)
        {
            try
            {
                var httpClient = _httpClient.CreateClient("TeamManagement");
                var team = await httpClient.GetFromJsonAsync<TeamResponseDTO>($"team/{engineerRequestDTO.TeamId.ToString()}");

                if (team is null)
                    throw new ArgumentException("The team not found!");

                var car = await httpClient.GetFromJsonAsync<TeamResponseDTO>($"car/{engineerRequestDTO.CarId.ToString()}");

                if (car is null)
                    throw new ArgumentException("The car not found!");

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

        public async Task UpdateActiveEngineer(EngineerUpdateRequestDTO engineerUpdateRequestDTO, Guid id)
        {
            try
            {
                var engineer = await _engineerRepository.GetEngineerByIdAsync(id);

                if (engineer is null)
                    throw new ArgumentException("The engineer not found");

                await _engineerRepository.UpdateActiveEngineerAsync(engineerUpdateRequestDTO, id);
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
