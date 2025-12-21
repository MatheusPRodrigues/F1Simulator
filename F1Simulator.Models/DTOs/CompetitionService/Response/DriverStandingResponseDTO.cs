namespace F1Simulator.Models.DTOs.CompetitionService.Response
{
    public class DriverStandingResponseDTO
    {
        public Guid DriverId { get; init; }
        public Guid TeamId { get; init; }
        public string DriverName { get; init; }
        public int Points { get; set; }
    }
}
