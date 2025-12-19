using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.DTOs.CompetitionService.Response
{
    public class RaceResponseDTO
    {
        public string NameCircuit { get; init; } 
        public string CountryCircuit { get; init; }
        public int Round { get; init; }
        public int Year { get; init; }
        public int LapsNumber { get; init; }
    }
}
