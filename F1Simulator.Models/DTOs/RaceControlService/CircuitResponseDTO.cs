using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.DTOs.RaceControlService
{
    public class CircuitResponseDTO
    {
        public string CircuitName { get; init; }
        public string Country { get; init; }
        public int LapsNumber { get; init; }
    }
}
