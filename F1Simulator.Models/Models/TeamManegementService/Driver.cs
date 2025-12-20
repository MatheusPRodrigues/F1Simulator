using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.Models.TeamManegement
{
    public class Driver
    {
        public Driver(int driverNumber, Guid teamId, Guid carId, string firstName, string fullName, double weightKg, double experienceFactor, double handicap)
        {
            DriverId = Guid.NewGuid();
            DriverNumber = driverNumber >= 1 && driverNumber <= 9 ? driverNumber.ToString("D2") : driverNumber.ToString();
            TeamId = teamId;
            CarId = carId;
            FirstName = firstName;
            FullName = fullName;
            WeightKg = weightKg;
            ExperienceFactor = experienceFactor;
            HandiCap = handicap;
            IsActive = true;
        }

        public Guid DriverId { get; set; }
        public double ExperienceFactor { get; set; }
        public string DriverNumber { get; set; }
        public Guid TeamId { get; set; }
        public Guid CarId { get; set; }
        public string FirstName { get; set; }
        public string FullName { get; set; }
        public double WeightKg { get; set; }
        public double HandiCap { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
