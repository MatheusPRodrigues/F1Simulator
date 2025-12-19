using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.DTOs.CompetitionService.Response
{
    public class RaceCompleteResponseDTO
    {
        public Guid Id { get; init; }
        public Guid SeasonId { get; init; }
        public Guid CircuitId { get; init; }
        public int Round { get; init; }
        public string Status { get; init; }
        public bool T1 { get; init; }
        public bool T2 { get; init; }
        public bool T3 { get; init; }
        public bool Qualifier { get; init; }
        public bool RaceFinal { get; init; }

    }
}
