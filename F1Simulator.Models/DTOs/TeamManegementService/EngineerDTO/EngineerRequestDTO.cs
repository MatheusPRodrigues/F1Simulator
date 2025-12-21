using F1Simulator.Models.Enums.TeamManegementService;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace F1Simulator.Models.DTOs.TeamManegementService.EngineerDTO
{
    public class EngineerRequestDTO
    {
        public Guid TeamId { get; init; }
        public Guid CarId { get; init; }
        public string FirstName { get; init; }
        public string FullName { get; init; }

        [JsonPropertyName("specialization")]
        [EnumDataType(typeof(Specialization))]
        public Specialization EngineerSpecialization { get; init; }
    }
}
