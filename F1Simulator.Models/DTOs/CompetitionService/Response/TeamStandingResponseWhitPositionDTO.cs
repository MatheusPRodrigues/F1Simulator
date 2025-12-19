using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.DTOs.CompetitionService.Response
{
    public class TeamStandingResponseWhitPositionDTO
    {
        public int Position { get; init; }
        public string TeamName { get; init; }
        public int Points { get; init; }
    }
}
