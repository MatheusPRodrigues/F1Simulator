using F1Simulator.Models.Enuns.TeamManegementService;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace F1Simulator.Models.DTOs.TeamManegementService.EngineerDTO
{
    public class EngineerRequestDTO
    {
        public Guid TeamId { get; init; }
        public Guid CarId { get; init; }
        public string FirstName { get; init; }
        public string FullName { get; init; }

        [EnumDataType(typeof(Specialization))]
        public Specialization EngineerSpecialization { get; init; }
    }
}
