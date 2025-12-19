using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.DTOs.TeamManegementService.BossDTO
{
    public class BossResponseDTO
    {
        public Guid BossId { get; init; }
        public Guid TeamId { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public int Age { get; init; }
    }
}
