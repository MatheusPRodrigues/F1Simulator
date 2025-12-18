using Dapper;
using F1Simulator.Models.DTOs.TeamManegementService.TeamDTO;
using F1Simulator.Models.Models.TeamManegement;
using F1Simulator.TeamManagementService.Data;
using F1Simulator.TeamManagementService.Services;
using F1Simulator.Utils.DatabaseConnectionFactory;
using Microsoft.Data.SqlClient;

namespace F1Simulator.TeamManagementService.Repositories
{
    public class TeamRepository
    {
        private readonly SqlConnection _connection;
        public TeamRepository(IDatabaseConnection<SqlConnection> connection)
        {
            _connection = connection.Connect();
        }

        public async Task<int> GetTeamsCountAsync()
        {
           

            var sql = @"SELECT COUNT(*) FROM Teams";

            return await _connection.ExecuteScalarAsync<int>(sql);
        }

        public async Task CreateTeamAsync(Team team)
        {
            var query = "INSERT INTO Teams (Name, NameAcronym, Country) " +
                        "VALUES (@Name, @NameAcronym, @Country)";

            await _connection.ExecuteAsync(query, team);
        }

        public async Task<IEnumerable<TeamResponseDTO>> GetAllTeamsAsync()
        {
                var query = @"SELECT TeamId, Name, NameAcronym, Country
                          FROM Teams";

                return await _connection.QueryAsync<TeamResponseDTO>(query);           
        }

        public async Task<TeamResponseDTO> GetTeamByIdAsync(string teamId)
        {
                var query = @"SELECT TeamId, Name, NameAcronym, Country
                          FROM Teams
                          WHERE TeamId = @TeamId";

                return await _connection.QueryFirstOrDefaultAsync<TeamResponseDTO>(query, new { TeamId = Guid.Parse(teamId) });           
        }
    }
}
