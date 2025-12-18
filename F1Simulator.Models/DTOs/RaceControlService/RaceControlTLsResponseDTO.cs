namespace F1Simulator.Models.DTOs.RaceControlService
{
    public class RaceControlTLsResponseDTO
    {
        public List<DriverComparisonResponseDTO> DriversComparison { get; init; } = new();
    }
}
