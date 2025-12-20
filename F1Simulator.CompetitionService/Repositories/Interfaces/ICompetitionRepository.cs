using F1Simulator.Models.DTOs.CompetitionService.Response;
using F1Simulator.Models.Models;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace F1Simulator.CompetitionService.Repositories.Interfaces
{
    public interface ICompetitionRepository
    {
        Task<SeasonResponseDTO?> GetCompetionActiveAsync();
        Task<int?> GetMaxYearSeasonAsync();
        Task StartSeasonAsync(Season season, List<TeamStanding> teams, List<DriverStanding> drivers, List<Race> races);
        Task<RaceCompleteResponseDTO?> GetRaceCompleteByIdAndSeasonIdAsync(int round, Guid seasonID);
        Task<bool> ExistRaceInProgressAsync(Guid seasonID);
        Task UpdateStatusRaceAsync(Guid id);
        Task<RaceResponseDTO?> GetRaceByIdAsync(Guid Id);
        Task<RaceWithCircuitResponseDTO?> GetRaceWithCircuitAsync();
        Task<RaceCompleteResponseDTO?> GetRaceInProgressAsync();
        Task UpdateRaceT1Async();
        Task UpdateRaceT2Async();
        Task UpdateRaceT3Async();
        Task UpdateRaceQualifierAsync();
        Task UpdateRaceRaceAsync();
        Task<List<DriverStandingResponseDTO>> GetDriverStandingAsync();
        Task<List<TeamStandingResponseDTO>> GetTeamStandingAsync();
        Task<List<RaceResponseDTO>> GetRacesAsync();
        Task EndRaceAsync(List<DriverStandingResponseDTO> driversUpdate,
            List<TeamStandingResponseDTO> teamsUpdate,
            SeasonResponseDTO season,
            RaceCompleteResponseDTO race);
    }
}
