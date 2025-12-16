using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.DTOs.TeamManegementService.TeamDTO
{
    public class TeamResponseDTO
    {
        public Guid TeamId { get; set; }
        public string Name { get; set; }
        public string NameAcronym { get; set; }
        public string Country { get; set; }
        public bool IsActive { get; set; }
    }
}
