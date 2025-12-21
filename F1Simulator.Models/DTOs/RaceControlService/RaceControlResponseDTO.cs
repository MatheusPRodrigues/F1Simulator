namespace F1Simulator.Models.DTOs.RaceControlService
{
    public class RaceControlResponseDTO
    {
        public string RaceId { get; init; }
        public int Round { get; init; }
        public int Season { get; init; }
        public CircuitResponseDTO Circuit { get; init; }
        public List<DriverQualifierResponseDTO> DriversQualifier { get; init; } = new();
        public List<DriverRaceResponseDTO> DriversRace { get; init; } = new();
    }
}
