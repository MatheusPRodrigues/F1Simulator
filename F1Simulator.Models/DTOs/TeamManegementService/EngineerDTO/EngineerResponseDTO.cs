using F1Simulator.Models.Enums.TeamManegementService;
using System.Text.Json.Serialization;

namespace F1Simulator.Models.DTOs.TeamManegementService.EngineerDTO
{
    public class EngineerResponseDTO
    {
        public Guid EngineerId { get; init; }
        public Guid TeamId { get; init; }
        public Guid CarId { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        [JsonIgnore]
        public Specialization EngineerSpecialization { get; init; }
        public string EngineerSpecializationDescription => EngineerSpecialization.ToString(); 
        public double ExperienceFactor { get; init; }
        public bool IsActive { get; init; }
    }
}
