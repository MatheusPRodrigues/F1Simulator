using F1Simulator.Models.DTOs.TeamManegementService.TeamDTO;

namespace F1Simulator.Models.DTOs.TeamManegementService.BossDTO
{
    public class BossWithTeamDTO
    {
        public Guid BossId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public TeamResponseDTO Team { get; set; }
    }
}
