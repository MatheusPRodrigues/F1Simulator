using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.DTOs.CompetitionService.Response
{
    public class RaceCalendarResponseDTO
    {
        public Guid RaceId { get; init; }
        public int Round { get; init; }
        public string Status { get; init; }
        public string CircuitName { get; init; }
        public string Conuntry { get; init; }
        public int Laps { get; init; }
    }
}
