using F1Simulator.Models.DTOs.TeamManegementService.DriverDTO;
using F1Simulator.TeamManagementService.Repositories.Interfaces;
using F1Simulator.TeamManagementService.Services.Interfaces;

namespace F1Simulator.TeamManagementService.Services
{
    public class DriverService : IDriverService
    {
        private readonly IDriverRepository _driverRepository;

        public DriverService(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        public Task<DriverResponseDTO> CreateDriverAsync(DriverRequestDTO driverRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DriverResponseDTO>> GetAllDriversAsync()
        {
            try
            {
                return await _driverRepository.GetAllDriversAsync();
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<DriverResponseDTO> GetDriverByIdAsync(Guid id)
        {
            try
            {
                return await _driverRepository.GetDriverByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<DriverResponseDTO> UpdateDriverAsync(DriveUpdaRequestDTO driverRequest)
        {
            try
            {
                return await _driverRepository.UpdateDriverAsync(driverRequest);
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
