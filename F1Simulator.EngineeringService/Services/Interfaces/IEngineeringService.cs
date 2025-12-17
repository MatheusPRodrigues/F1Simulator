using F1Simulator.Models.DTOs.EngineeringService;
using F1Simulator.Models.DTOs.TeamManegementService.CarDTO;

namespace F1Simulator.EngineeringService.Services.Interfaces
{
    public interface IEngineeringService
    {
        Task<CarResponseDTO> PutCarCoefficientsAsync(EngineersPutDTO? engIds, string carId);
    }
}
