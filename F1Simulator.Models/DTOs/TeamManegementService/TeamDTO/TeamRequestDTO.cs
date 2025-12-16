using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.DTOs.TeamManegementService.TeamDTO
{
    public class TeamRequestDTO
    {
        public string Name { get; set; }
        public string NameAcronym { get; set; }
        public string Country { get; set; }
    }
}
