using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.DTOs.TeamManegementService.CarDTO
{
    public class CarRequestDTO
    {
        public string TeamId { get; init; }
        public double WeightKg { get; init; }
        public int Speed { get; init; }
    }
}
