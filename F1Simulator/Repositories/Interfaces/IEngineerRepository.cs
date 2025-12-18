using F1Simulator.Models.DTOs.TeamManegementService.EngineerDTO;
using F1Simulator.Models.Models.TeamManegement;

namespace F1Simulator.TeamManagementService.Repositories.Interfaces
{
    public interface IEngineerRepository
    {
        public Task<EngineerResponseDTO> CreateEngineerAsync(Engineer engineerRequest);
        public Task<List<EngineerResponseDTO>> GetAllEnginnersAsync();
        public Task<EngineerResponseDTO> GetEngineerByIdAsync(Guid id);
        public Task UpdateActiveEngineerAsync(EngineerUpdateRequestDTO engineerUpdateRequestDTO, Guid id);
    }
}
