using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.DTOs.CompetitionService.Request
{
    public class CreateCircuitRequestDTO
    {
        public string Name { get; init; }
        public string Country { get; init; }     
        public int LapsNumber { get; init; }       
    }
}
