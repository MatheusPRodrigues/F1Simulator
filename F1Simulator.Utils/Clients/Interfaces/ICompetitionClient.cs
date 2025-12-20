using F1Simulator.Models.DTOs.CompetitionService.Response;

namespace F1Simulator.Utils.Clients.Interfaces
{
    public interface ICompetitionClient
    {
        Task<SeasonResponseDTO> GetActiveSeasonAsync();
    }
}
