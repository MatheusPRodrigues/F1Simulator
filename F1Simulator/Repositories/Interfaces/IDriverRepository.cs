using F1Simulator.Models.DTOs.TeamManegementService.DriverDTO;
using F1Simulator.Models.Models.TeamManegement;

namespace F1Simulator.TeamManagementService.Repositories.Interfaces
{
    public interface IDriverRepository
    {
        public Task<DriverResponseDTO> CreateDriverAsync(Driver driver);
        public Task<DriverResponseDTO> UpdateDriverAsync(Driver driver);
        public Task<List<DriverResponseDTO>> GetAllDriversAsync();
        public Task<DriverResponseDTO> GetDriverByIdAsync(Guid id);
    }
}
