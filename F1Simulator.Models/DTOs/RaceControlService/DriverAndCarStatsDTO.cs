using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.DTOs.RaceControlService
{
    public class DriverAndCarStatsDTO
    {
        public string DriverId { get; init; }
        public string DriverName { get; init; }
        public double Handicap { get; init; }
        public string CarId { get; init; }
        public double Ca { get; init; }
        public double Cp { get; init; }
    }
}
