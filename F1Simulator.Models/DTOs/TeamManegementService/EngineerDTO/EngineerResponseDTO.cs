using F1Simulator.Models.Enuns.TeamManegementService;
using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.DTOs.TeamManegementService.EngineerDTO
{
    public class EngineerResponseDTO
    {
        public Guid EngineerId { get; set; }
        public Guid TeamId { get; set; }
        public Guid CarId { get; set; }
        public string FirstName { get; set; }
        public string FullName { get; set; }
        public Specialization EngineerSpecialization { get; set; }
        public int Age { get; set; }
        public double ExperienceFactor { get; set; }
        public bool IsActive { get; set; }
    }
}
