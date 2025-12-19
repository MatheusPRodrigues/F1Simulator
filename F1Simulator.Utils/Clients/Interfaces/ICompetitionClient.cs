using F1Simulator.Models.DTOs.CompetitionService.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Utils.Clients.Interfaces
{
    public interface ICompetitionClient
    {
        Task<SeasonResponseDTO> GetActiveSeasonAsync();
    }
}
