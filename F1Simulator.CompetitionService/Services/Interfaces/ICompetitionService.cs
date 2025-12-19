using F1Simulator.CompetitionService.Repositories;
using F1Simulator.Models.DTOs.CompetitionService.Response;
using System.Threading.Tasks;

namespace F1Simulator.CompetitionService.Services.Interfaces
{
    public interface ICompetitionService
    {
        Task<SeasonResponseDTO?> GetCompetitionActiveAsync();
        Task<SeasonResponseDTO?> StartSeasonAsync();
        Task<RaceResponseDTO?> StartRaceAsync(int round);
        Task<RaceWithCircuitResponseDTO?> GetRaceWithCircuitAsync();
        Task UpdateRaceT1Async();
        Task UpdateRaceT2Async();
        Task UpdateRaceT3Async();
        Task UpdateRaceQualifierAsync();
        Task UpdateRaceRaceAsync();
        Task<List<DriverStandingResponseWhitPositionDTO>> GetDriverStandingAsync();
        Task<List<TeamStandingResponseWhitPositionDTO>> GetTeamStandingAsync();
        Task<List<RaceResponseDTO>> GetRacesAsync();
    }
}
