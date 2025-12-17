using F1Simulator.Models.Enuns.TeamManegementService;
using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.DTOs.TeamManegementService.EngineerDTO
{
    public class EngineerResponseDTO
    {
        public string EngineerId { get; init; }
        public string TeamId { get; init; }
        public string CarId { get; init; }
        public string FirstName { get; init; }
        public string FullName { get; init; }
        public Specialization EngineerSpecialization { get; init; }
        public int Age { get; init; }
        public double ExperienceFactor { get; init; }
        public bool IsActive { get; init; }
    }
}
