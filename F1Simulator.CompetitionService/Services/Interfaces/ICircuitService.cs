using F1Simulator.Models.DTOs.CompetitionService.Request;
using F1Simulator.Models.DTOs.CompetitionService.Response;
using F1Simulator.Models.DTOs.CompetitionService.Update;

namespace F1Simulator.CompetitionService.Services.Interfaces
{
    public interface ICircuitService
    {
        Task<CreateCircuitResponseDTO> CreateCircuitAsync(CreateCircuitRequestDTO createCircuit);
        Task<CreateCircuitsResponseDTO> CreateCircuitsAsync(CreateCircuitsRequestDTO circuits);
        Task<(bool Update, CreateCircuitResponseDTO? Circuit)> DeactivateCircuitAsync(Guid id);
        Task<(bool Update, CreateCircuitResponseDTO? Circuit)> ActivateCircuitAsync(Guid id);
        Task<List<CreateCircuitResponseDTO>> GetAllCircuits();
        Task<CreateCircuitResponseDTO?> GetCircuitById(Guid id);
        Task<bool> DeleteCircuitAsync(Guid id);
        Task<(bool Update, CreateCircuitResponseDTO? Circuit)> UpdateCircuitAsync(Guid id, UpdateCircuitDTO updateCircuit);


    }
}
