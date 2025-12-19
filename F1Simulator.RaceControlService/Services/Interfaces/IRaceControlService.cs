using F1Simulator.Models.DTOs.RaceControlService;

namespace F1Simulator.RaceControlService.Services.Interfaces
{
    public interface IRaceControlService
    {
        Task ExecuteTlOneSectionAsync(string raceId);
        Task ExecuteTlTwoSectionAsync(string raceId);
        Task ExecuteTlThreeSectionAsync(string raceId);
        Task ExecuteQualifierSectionAsync(string raceId);
        Task<List<DriverGridFinalRaceResponseDTO>> ExecuteRaceSectionAsync(string raceId);
    }
}
