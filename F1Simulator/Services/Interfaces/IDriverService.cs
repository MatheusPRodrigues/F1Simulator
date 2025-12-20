using F1Simulator.Models.DTOs.RaceControlService;
using F1Simulator.Models.DTOs.TeamManegementService.DriverDTO;
using F1Simulator.Models.Models.TeamManegement;

namespace F1Simulator.TeamManagementService.Services.Interfaces
{
    public interface IDriverService
    {
        public Task<DriverResponseDTO> CreateDriverAsync(DriverRequestDTO driverRequest);
        public Task UpdateDriverAsync(Guid id, UpdateRequestDriverDTO driverRequest);
        public Task<List<DriverResponseDTO>> GetDriversAsync();
        public Task<DriverResponseDTO> GetDriverByIdAsync(Guid id);
        public Task<List<DriverToRaceDTO>> GetDriversToRaceAsync();
        public Task<int> GetAllDriversCount();
    }
}
