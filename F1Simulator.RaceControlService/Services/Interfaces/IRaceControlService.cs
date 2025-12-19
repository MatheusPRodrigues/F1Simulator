using F1Simulator.Models.DTOs.RaceControlService;

namespace F1Simulator.RaceControlService.Services.Interfaces
{
    public interface IRaceControlService
    {
        Task<List<DriverComparisonResponseDTO>> ExecuteTlOneSectionAsync(string raceId);
        Task<List<DriverComparisonResponseDTO>> ExecuteTlTwoSectionAsync(string raceId);
        Task<List<DriverComparisonResponseDTO>> ExecuteTlThreeSectionAsync(string raceId);
        Task<List<DriverGridResponseDTO>> ExecuteQualifierSectionAsync(string raceId);
        Task<List<DriverGridFinalRaceResponseDTO>> ExecuteRaceSectionAsync(string raceId);
    }
}
