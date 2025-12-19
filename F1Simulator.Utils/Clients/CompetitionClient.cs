using DnsClient.Internal;
using F1Simulator.Models.DTOs.CompetitionService.Response;
using F1Simulator.Utils.Clients.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace F1Simulator.Utils.Clients
{
    public class CompetitionClient : ICompetitionClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CompetitionClient> _logger;

        public CompetitionClient(HttpClient httpClient, ILogger<CompetitionClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<SeasonResponseDTO?> GetActiveSeasonAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/competition/session/active");

                if (!response.IsSuccessStatusCode)
                    return null;

                return await response.Content.ReadFromJsonAsync<SeasonResponseDTO>();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error calling CompetitionService");
                throw;
            }
        }
    }
}
