using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.DTOs.TeamManegementService.DriverDTO
{
    public class DriverRequestDTO
    {
        public Guid TeamId { get; set; }
        public Guid CarId { get; set; }
        public string FirstName { get; set; }
        public string FullName { get; set; }
        public string NameAcronym { get; set; }
        public decimal WeightKg { get; set; }
    }
}
