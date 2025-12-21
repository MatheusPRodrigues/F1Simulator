namespace F1Simulator.Models.DTOs.CompetitionService.Response
{
    public class CreateCircuitsResponseDTO
    {
        public List<CreateCircuitResponseDTO> circuits { get; init; } = new List<CreateCircuitResponseDTO>();
    }
}
