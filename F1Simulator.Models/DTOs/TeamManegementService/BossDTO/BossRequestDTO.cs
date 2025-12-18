using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.DTOs.TeamManegementService.BossDTO
{
    internal class BossRequestDTO
    {
        public string TeamId { get; init; }
        public string FirstName { get; init; }
        public string FullName { get; init; }
        public int Age { get; init; }
    }
}
