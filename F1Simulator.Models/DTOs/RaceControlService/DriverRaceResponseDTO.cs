using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.DTOs.RaceControlService
{
    public class DriverRaceResponseDTO
    {
        public string DriverName { get; init; }
        public string TeamName { get; init; }
        public int Position { get; init; }
        public int Pontuation { get; init; }
    }
}
