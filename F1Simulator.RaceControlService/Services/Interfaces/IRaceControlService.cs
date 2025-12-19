using F1Simulator.Models.DTOs.RaceControlService;

namespace F1Simulator.RaceControlService.Services.Interfaces
{
    public interface IRaceControlService
    {
        Task<List<DriverComparisonResponseDTO>> ExecuteTlOneSectionAsync();
        Task<List<DriverComparisonResponseDTO>> ExecuteTlTwoSectionAsync();
        Task<List<DriverComparisonResponseDTO>> ExecuteTlThreeSectionAsync();
        Task<List<DriverGridResponseDTO>> ExecuteQualifierSectionAsync();
        Task<List<DriverGridFinalRaceResponseDTO>> ExecuteRaceSectionAsync();
    }
}
