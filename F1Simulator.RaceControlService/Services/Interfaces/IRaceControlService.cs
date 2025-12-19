using F1Simulator.Models.DTOs.RaceControlService;
using F1Simulator.Models.Models.RaceControlService;

namespace F1Simulator.RaceControlService.Services.Interfaces
{
    public interface IRaceControlService
    {
        Task<List<DriverComparisonResponseDTO>> ExecuteTlOneSectionAsync();
        Task<List<DriverComparisonResponseDTO>> ExecuteTlTwoSectionAsync();
        Task<List<DriverComparisonResponseDTO>> ExecuteTlThreeSectionAsync();
        Task<List<DriverGridResponseDTO>> ExecuteQualifierSectionAsync();
        Task<List<DriverGridFinalRaceResponseDTO>> ExecuteRaceSectionAsync();
        Task<RaceControlResponseDTO> GetRaceByRaceIdAsync(Guid raceId);
        Task<List<RaceControlResponseDTO>> GetRacesBySeasonYearAsync(int year);
    }
}
