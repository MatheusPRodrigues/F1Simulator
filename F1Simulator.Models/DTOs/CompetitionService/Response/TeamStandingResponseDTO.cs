namespace F1Simulator.Models.DTOs.CompetitionService.Response
{
    public class TeamStandingResponseDTO
    {
        public Guid TeamId { get; init; }
        public string TeamName { get; init; }
        public int Points { get; set; }
    }
}
