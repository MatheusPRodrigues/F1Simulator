using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.DTOs.TeamManegementService.BossDTO
{
    internal class BossRequestDTO
    {
        public Guid TeamId { get; set; }
        public string FirstName { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
    }
}
