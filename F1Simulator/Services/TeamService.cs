using F1Simulator.Models.DTOs.TeamManegementService.TeamDTO;
using F1Simulator.Models.Models.TeamManegement;
using F1Simulator.TeamManagementService.Data;
using F1Simulator.TeamManagementService.Repositories;
using F1Simulator.TeamManagementService.Services.Interfaces;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Data.SqlClient;

namespace F1Simulator.TeamManagementService.Services
{
    public class TeamService : ITeamService
    {
        private readonly TeamManagementServiceConnection _connection;
        private readonly ILogger<Team> _logger;
        private readonly TeamRepository _teamRepository;
        public TeamService( ILogger<Team> logger,TeamManagementServiceConnection connection, TeamRepository teamRepository)
        {
            _logger = logger;
            _teamRepository = teamRepository;
            _connection = connection;
        }

        public async Task<int> GetTeamsCountAsync()
        {
            try
            {
                return await _teamRepository.GetTeamsCountAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting count the team: {ex.Message}", ex);
                throw;
            }
        }
        private static string GenerateAcronym(string name)
        {
            if (name is null || name.Length < 3)
                throw new Exception("Team name must have at least 3 characters!");

            return name.Trim().Substring(0, 3).ToUpperInvariant();
        }
        public async Task CreateTeamAsync(TeamRequestDTO teamRequestDto)
        {
            try
            {
                var count = await _teamRepository.GetTeamsCountAsync();

                if (count > 11)
                    throw new Exception("Max teams reached!");

                var existingTeam = await _teamRepository.GetTeamByNameAsync(teamRequestDto.Name);
                if (existingTeam is not null)
                    throw new Exception("Team is already exists!");

                var acronym = GenerateAcronym(teamRequestDto.Name);

                var team = new Team
                {
                    TeamId = Guid.NewGuid(),
                    Name = teamRequestDto.Name.Trim(),
                    NameAcronym = acronym,
                    Country = teamRequestDto.Country
                };

                await _teamRepository.CreateTeamAsync(team);
            }
            catch(Exception ex)
            {
                _logger.LogError($"An error occurred while creating the team: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<List<TeamResponseDTO>> GetAllTeamsAsync()
        {
            try
            {
                var teams = await _teamRepository.GetAllTeamsAsync();
                return teams.ToList();
            }
            catch(Exception ex)
            {
                _logger.LogError($"An error occurred while getting the team list: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<TeamResponseDTO> GetTeamByIdAsync(string teamId)
        {
            try
            {
                if (!Guid.TryParse(teamId, out var idGuid))
                    throw new Exception("Invalid team id!");

                var team = await _teamRepository.GetTeamByIdAsync(idGuid);

                if (team is null)
                    throw new Exception("Team not found!");

                return team;
            }
            catch(Exception ex)
            {
                _logger.LogError($"An error occurred while getting team by id: {ex.Message}", ex);
                throw;
            }
        }
        public async Task<TeamResponseDTO> GetTeamByNameAsync(string name)
        {
            try
            {
                var team = await _teamRepository.GetTeamByNameAsync(name);

                if (team is null)
                    throw new Exception("Team not found!");

                return team;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting team by name: {ex.Message}", ex);
                throw;
            }
        }
        public async Task UpdateTeamCountryAsync(string teamId, string country)
        {
            try
            {
                if (!Guid.TryParse(teamId, out var idGuid))
                    throw new Exception("Invalid team id!");

                if (country is null)
                    throw new Exception("Country can´t be empty!");

                var team = await _teamRepository.GetTeamByIdAsync(idGuid);
                if (team is null)
                    throw new Exception("Team not found!");

                await _teamRepository.UpdateTeamCountryAsync(idGuid, country);
            }
            catch(Exception ex)
            {
                _logger.LogError($"An error occurred while updatting team: {ex.Message}", ex);
                throw;
            }
        }
    }
}
