using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.DTOs.TeamManegementService.BossDTO
{
    public class BossResponseDTO
    {
        public string BossId { get; init; }
        public string TeamId { get; init; }
        public string FirstName { get; init; }
        public string FullName { get; init; }
        public int Age { get; init; }
        public bool IsActive { get; init; }
    }
}
