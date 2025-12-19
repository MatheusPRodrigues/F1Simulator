using F1Simulator.Models.DTOs.TeamManegementService.TeamDTO;
using F1Simulator.Models.Models.TeamManegement;

namespace F1Simulator.TeamManagementService.Repositories.Interfaces
{
    public interface ITeamRepository
    {
        Task<int> GetTeamsCountAsync();
        Task CreateTeamAsync(Team team);
        Task<IEnumerable<TeamResponseDTO>> GetAllTeamsAsync();
        Task<TeamResponseDTO> GetTeamByIdAsync(Guid teamId);
        Task<TeamResponseDTO> GetTeamByNameAsync(string name);
        Task UpdateTeamCountryAsync(Guid teamId, string country);
        Task<int> GetDriversInTeamById(Guid id);
    }
}
