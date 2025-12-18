using F1Simulator.Models.Enuns.TeamManegementService;
using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.DTOs.TeamManegementService.EngineerDTO
{
    public class EngineerResponseDTO
    {
        public Guid EngineerId { get; init; }
        public Guid TeamId { get; init; }
        public Guid CarId { get; init; }
        public string FirstName { get; init; }
        public string FullName { get; init; }
        public Specialization EngineerSpecialization { get; init; }
        public double ExperienceFactor { get; init; }
        public bool IsActive { get; init; }
    }
}
