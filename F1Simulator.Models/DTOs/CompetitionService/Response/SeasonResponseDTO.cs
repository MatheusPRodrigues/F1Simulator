using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.DTOs.CompetitionService.Response
{
    public class SeasonResponseDTO
    {
        public Guid Id { get; init; }
        public int Year { get; init; }
        public bool IsActive { get; init; }
    }
}
