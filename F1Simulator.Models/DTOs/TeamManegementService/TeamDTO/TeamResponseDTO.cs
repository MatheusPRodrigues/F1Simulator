using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.DTOs.TeamManegementService.TeamDTO
{
    public class TeamResponseDTO
    {
        public string TeamId { get; init; }
        public string Name { get; init; }
        public string NameAcronym { get; init; }
        public string Country { get; init; }
    }
}
