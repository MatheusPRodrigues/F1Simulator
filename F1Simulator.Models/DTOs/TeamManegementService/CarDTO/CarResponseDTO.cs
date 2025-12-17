using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.DTOs.TeamManegementService.CarDTO
{
    public class CarResponseDTO
    {
        public string CarId { get; init; }
        public string TeamId { get; init; }
        public string Model { get; init; }
        public double WeightKg { get; init; }
        public int Speed { get; init; }
        public double Ca { get; init; }
        public double Cp { get; init; }
    }
}
