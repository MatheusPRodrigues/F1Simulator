using Dapper;
using F1Simulator.Models.DTOs.TeamManegementService.TeamDTO;
using F1Simulator.Models.Models.TeamManegement;
using F1Simulator.TeamManagementService.Data;
using F1Simulator.TeamManagementService.Services;
using Microsoft.Data.SqlClient;

namespace F1Simulator.TeamManagementService.Repositories
{
    public class TeamRepository
    {
        private readonly TeamManagementServiceConnection _connection;
        public TeamRepository(TeamManagementServiceConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> GetTeamsCountAsync()
        {
            using var connection = _connection.GetConnection();

            var sql = @"SELECT COUNT(*) FROM Teams";

            return await connection.ExecuteScalarAsync<int>(sql);
        }

        public async Task CreateTeamAsync(Team team)
        {
            var query = "INSERT INTO Teams (Name, NameAcronym, Country) " +
                        "VALUES (@Name, @NameAcronym, @Country)";

            using var connection = _connection.GetConnection();
            await connection.ExecuteAsync(query, team);
        }

        public async Task<IEnumerable<TeamResponseDTO>> GetAllTeamsAsync()
        {
                using var connection = _connection.GetConnection();
                var query = @"SELECT TeamId, Name, NameAcronym, Country
                          FROM Teams";

                return await connection.QueryAsync<TeamResponseDTO>(query);           
        }

        public async Task<TeamResponseDTO> GetTeamByIdAsync(string teamId)
        {
                using var connection = _connection.GetConnection();
                var query = @"SELECT TeamId, Name, NameAcronym, Country
                          FROM Teams
                          WHERE TeamId = @TeamId";

                return await connection.QueryFirstOrDefaultAsync<TeamResponseDTO>(query, new { TeamId = Guid.Parse(teamId) });           
        }
    }
}
