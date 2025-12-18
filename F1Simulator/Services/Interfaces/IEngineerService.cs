using F1Simulator.Models.DTOs.TeamManegementService.EngineerDTO;

namespace F1Simulator.TeamManagementService.Services.Interfaces
{
    public interface IEngineerService
    {
        public Task<EngineerResponseDTO> CreateEngineerAsync(EngineerRequestDTO engineerRequestDTO);
        public Task<List<EngineerResponseDTO>> GetAllEngineersAsync();
        public Task<EngineerResponseDTO> GetEngineerByIdAsync(Guid id);
        public Task UpdateActiveEngineer(EngineerUpdateRequestDTO engineerUpdateRequestDTO, Guid id);
    }
}
