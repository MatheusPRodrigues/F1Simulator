using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.DTOs.TeamManegementService.CarDTO
{
    public class CarRequestDTO
    {
        public Guid TeamId { get; set; }
        public string Model { get; set; }
        public decimal WeightKg { get; set; }
        public int Speed { get; set; }
    }
}
