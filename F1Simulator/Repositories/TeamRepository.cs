using Dapper;
using F1Simulator.Models.DTOs.TeamManegementService.TeamDTO;
using F1Simulator.Models.Models.TeamManegement;
using F1Simulator.TeamManagementService.Repositories.Interfaces;
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
            try
            {
                var sql = @"SELECT COUNT(*) FROM Teams";

                return await _connection.ExecuteScalarAsync<int>(sql);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> GetDriversInTeamByIdAsync(Guid id)
        {
            try
            {
                var sql = @"SELECT COUNT(*) FROM Teams t
                            INNER JOIN Drivers d 
                            ON t.TeamId = d.TeamId WHERE t.TeamId = @Id";

                return await _connection.ExecuteScalarAsync<int>(sql, new { Id = id});
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task CreateTeamAsync(Team team)
        {
            try
            {
                var query = "INSERT INTO Teams (TeamId, Name, NameAcronym, Country) " +
                        "VALUES (@TeamId, @Name, @NameAcronym, @Country)";

                await _connection.ExecuteAsync(query, team);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<TeamResponseDTO>> GetAllTeamsAsync()
        {
            try
            {
                var query = @"SELECT TeamId, Name, NameAcronym, Country
                          FROM Teams";

                return await _connection.QueryAsync<TeamResponseDTO>(query);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TeamResponseDTO> GetTeamByIdAsync(Guid teamId)
        {
            try
            {
                var query = @"SELECT TeamId, Name, NameAcronym, Country
                          FROM Teams
                          WHERE TeamId = @TeamId";

                return await _connection.QueryFirstOrDefaultAsync<TeamResponseDTO>(query, new { TeamId = teamId });
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TeamResponseDTO> GetTeamByNameAsync(string name)
        {
            try
            {
                var query = @"SELECT TeamId, Name, NameAcronym, Country
                          FROM Teams
                          WHERE Name = @Name";

                return await _connection.QueryFirstOrDefaultAsync<TeamResponseDTO>(query, new { Name = name });

            }
            catch (SqlException ex)
            {
                throw new Exception("Error querying the database.", ex);
            }
        }

        public async Task UpdateTeamCountryAsync(Guid teamId, TeamCountryDTO team)
        {
            try
            {
                var query = @"UPDATE Teams
                          SET Country = @Country
                          WHERE TeamId = @TeamId";

                await _connection.ExecuteAsync(query, new
                {
                    TeamId = teamId,
                    Country = team.Country
                });
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
