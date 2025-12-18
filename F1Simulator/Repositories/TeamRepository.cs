using Dapper;
using F1Simulator.Models.DTOs.TeamManegementService.TeamDTO;
using F1Simulator.Models.Models.TeamManegement;
using F1Simulator.TeamManagementService.Data;
using F1Simulator.TeamManagementService.Repositories.Interfaces;
using F1Simulator.TeamManagementService.Services;
using F1Simulator.Utils.DatabaseConnectionFactory;
using Microsoft.Data.SqlClient;

namespace F1Simulator.TeamManagementService.Repositories
{
    public class TeamRepository : ITeamRepository
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
            var query = "INSERT INTO Teams (TeamId, Name, NameAcronym, Country) " +
                        "VALUES (@TeamId, @Name, @NameAcronym, @Country)";

            await _connection.ExecuteAsync(query, team);
        }

        public async Task<IEnumerable<TeamResponseDTO>> GetAllTeamsAsync()
        {
                var query = @"SELECT TeamId, Name, NameAcronym, Country
                          FROM Teams";

                return await _connection.QueryAsync<TeamResponseDTO>(query);           
        }

        public async Task<TeamResponseDTO> GetTeamByIdAsync(Guid teamId)
        {
                var query = @"SELECT TeamId, Name, NameAcronym, Country
                          FROM Teams
                          WHERE TeamId = @TeamId";

                return await _connection.QueryFirstOrDefaultAsync<TeamResponseDTO>(query, new { TeamId = teamId });           
        }

        public async Task<TeamResponseDTO> GetTeamByNameAsync(string name)
        {
                var query = @"SELECT TeamId, Name, NameAcronym, Country
                          FROM Teams
                          WHERE Name = @Name";

                return await _connection.QueryFirstOrDefaultAsync<TeamResponseDTO>(query, new { Name = name });           
        }

        public async Task UpdateTeamCountryAsync(Guid teamId, string country)
        {
            var query = @"UPDATE Teams
                          SET Country = @Country
                          WHERE TeamId = @TeamId";

            await _connection.ExecuteAsync(query, new 
            {
                TeamId = teamId,
                Country = country
            });
        }
    }
}
