using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.DTOs.CompetitionService.Response
{
    public class DriverStandingResponseWhitPositionDTO
    {

        public int Position { get; init; }
        public string DriverName { get; init; }
        public decimal Points { get; init; }
    }
}
