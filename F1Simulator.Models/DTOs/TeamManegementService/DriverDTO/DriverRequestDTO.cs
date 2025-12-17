using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.DTOs.TeamManegementService.DriverDTO
{
    public class DriverRequestDTO
    {
        public string TeamId { get; init; }
        public string CarId { get; init; }
        public string FirstName { get; init; }
        public string FullName { get; init; }
        public string NameAcronym { get; init; }
        public double WeightKg { get; init; }
    }
}
