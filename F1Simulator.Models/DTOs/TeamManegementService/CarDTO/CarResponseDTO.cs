using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.DTOs.TeamManegementService.CarDTO
{
    public class CarResponseDTO
    {
        public Guid CarId { get; set; }
        public Guid TeamId { get; set; }
        public string Model { get; set; }
        public decimal WeightKg { get; set; }
        public int Speed { get; set; }
        public double Ca { get; set; }
        public double Cp { get; set; }
        public bool IsActive { get; set; } 
    }
}
