using Microsoft.AspNetCore.Mvc;

namespace F1Simulator.EngineeringService.Services.Interfaces
{
    public interface IEngineeringService
    {
        Task<CarResultDTO> PutCarCoefficientsAsync(EngineersRequestDTO eng, string carId);
    }
}
