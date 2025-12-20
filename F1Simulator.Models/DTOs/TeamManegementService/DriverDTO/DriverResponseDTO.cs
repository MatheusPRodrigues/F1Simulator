using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.DTOs.TeamManegementService.DriverDTO
{
    public class DriverResponseDTO
    {
        public Guid? DriverId { get; init; }
        public string DriverNumber { get; init; }
        public Guid TeamId { get; init; }
        public Guid CarId { get; init; }
        public string FirstName { get; init; }
        public string FullName { get; init; }
        public double WeightKg { get; init; }
        public double HandiCap { get; init; }
        public bool IsActive { get; init; }
        public double ExperienceFactor { get; set; }
    }
}
