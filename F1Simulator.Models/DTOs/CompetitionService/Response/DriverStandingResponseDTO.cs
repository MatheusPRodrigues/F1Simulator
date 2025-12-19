using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.DTOs.CompetitionService.Response
{
    public class DriverStandingResponseDTO
    {
        public string DriverId { get; init; }
        public string DriverName { get; init; }
        public decimal Points { get; init; }
    }
}
