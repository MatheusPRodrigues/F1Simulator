using F1Simulator.Models.DTOs.TeamManegementService.TeamDTO;

namespace F1Simulator.TeamManagementService.Services.Interfaces
{
    public interface ITeamService
    {
        Task<int> GetTeamsCountAsync();
        Task CreateTeamAsync(TeamRequestDTO teamRequestDto);
        Task<List<TeamResponseDTO>> GetAllTeamsAsync();
        Task<TeamResponseDTO> GetTeamByIdAsync(string teamId);
        Task<TeamResponseDTO> GetTeamByNameAsync(string name);
        Task UpdateTeamCountryAsync(string teamId, string country);
    }
}
