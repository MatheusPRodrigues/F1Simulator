namespace F1Simulator.Models.DTOs.RaceControlService
{
    public class DriverComparisonResponseDTO
    {
        public DriverAndCarStatsDTO OlderStats { get; init; }
        public DriverAndCarStatsDTO NewStats { get; init; }
    }
}
