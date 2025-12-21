using F1Simulator.Models.DTOs.TeamManegementService.TeamDTO;

namespace F1Simulator.Models.DTOs.TeamManegementService.BossDTO
{
    public class BossWithTeamDTO
    {
        public Guid BossId { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public int Age { get; init; }
        public TeamResponseDTO Team { get; set; }
    }
}
