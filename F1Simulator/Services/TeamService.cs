using F1Simulator.Models.DTOs.TeamManegementService.TeamDTO;
using F1Simulator.Models.Models.TeamManegement;
using F1Simulator.TeamManagementService.Data;
using F1Simulator.TeamManagementService.Repositories;
using Microsoft.Data.SqlClient;

namespace F1Simulator.TeamManagementService.Services
{
    public class TeamService
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
        public async Task CreateTeamAsync(TeamRequestDTO teamRequestDto)
        {
            try
            {
                var count = await _teamRepository.GetTeamsCountAsync();

                if (count > 11)
                    throw new Exception("Max teams reached!");

                var team = new Team
                {
                    TeamId = Guid.NewGuid(),
                    Name = teamRequestDto.Name,
                    NameAcronym = teamRequestDto.NameAcronym,
                    Country = teamRequestDto.Country
                };

                await _teamRepository.CreateTeamAsync(team);
            }
            catch(Exception ex)
            {
                _logger.LogError($"An error occurred while creating the team: {ex.Message}", ex);
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
                var team = await _teamRepository.GetTeamByIdAsync(teamId);

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
    }
}
