using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.DTOs.CompetitionService.Request
{
    public class CreateCircuitsRequestDTO
    {
        public List<CreateCircuitRequestDTO> CircuitsRequest { get; init; } = new List<CreateCircuitRequestDTO>();
    }
}
