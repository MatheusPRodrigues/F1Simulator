using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.DTOs.CompetitionService.Response
{
    public class CircuitCompleteResponseDTO
    {
        public Guid CircuitId { get; init; }
        public string Name { get; init; }
        public string Country { get; init; }
        public int LapsNumber { get; init; }
        public bool IsActive { get; init; }
    }
}
