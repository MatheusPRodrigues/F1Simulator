namespace F1Simulator.Models.DTOs.CompetitionService.Response
{
    public class RaceWithCircuitResponseDTO
    {

        public Guid Id { get; init; }
        public int YearSeason { get; init; }
        public int Round { get; init; }
        public string Status { get;  init; }
        public bool T1 { get; init; }
        public bool T2 { get; init; }
        public bool T3 { get; init; }
        public bool Qualifier { get; init; }
        public bool RaceFinal { get; init; }
        public CircuitCompleteResponseDTO Circuit { get; set; } = new CircuitCompleteResponseDTO();

    }
}
