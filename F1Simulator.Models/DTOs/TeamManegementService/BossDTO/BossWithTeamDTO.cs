using F1Simulator.Models.DTOs.TeamManegementService.TeamDTO;
using System;
using System.Collections.Generic;
using System.Text;

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
