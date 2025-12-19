using Dapper;
using F1Simulator.Models.DTOs.CompetitionService.Response;
using F1Simulator.Models.Models;

namespace F1Simulator.CompetitionService.Repositories.Interfaces
{
    public interface ICircuitRepository
    {
        Task CreateCircuitAsync(Circuit createCircuit);
        Task<bool> CircuitExistsAsync(string name);
        Task<int> CircuitsActivatesAsync();
        Task<CreateCircuitResponseDTO?> GetCircuitById(Guid id);
        Task UpdateIsActiveCircuitAsync(Guid id);
        Task<List<CreateCircuitResponseDTO>> GetAllCircuitsAsync();
        Task<CreateCircuitResponseDTO?> GetCircuitByIdAsync(Guid id);
        Task<bool> DeleteCircuitAsync(Guid id);
        Task<bool> UpdateCircuitAsync(Guid id, List<string> updates, DynamicParameters parameters);
        Task<List<CreateCircuitResponseDTO>> GetAllCircuitsActiveAsync();


    }
}
