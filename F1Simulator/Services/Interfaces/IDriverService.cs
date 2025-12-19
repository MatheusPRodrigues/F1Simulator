using F1Simulator.Models.DTOs.TeamManegementService.DriverDTO;

namespace F1Simulator.TeamManagementService.Services.Interfaces
{
    public interface IDriverService
    {
        public Task<DriverResponseDTO> CreateDriverAsync(DriverRequestDTO driverRequest);
        public Task<DriverResponseDTO> UpdateDriverAsync(DriverRequestDTO driverRequest);
        public Task<List<DriverResponseDTO>> GetAllDriversAsync();
        public Task<DriverResponseDTO> GetDriverByIdAsync(Guid id);
    }
}
