namespace F1Simulator.Models.DTOs.TeamManegementService.TeamDTO
{
    public class TeamResponseDTO
    {
        public Guid TeamId { get; init; }
        public string Name { get; init; }
        public string NameAcronym { get; init; }
        public string Country { get; init; }
    }
}
