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
            try
            {
                var sql = @"SELECT COUNT(*) FROM Teams";

                return await _connection.ExecuteScalarAsync<int>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> GetDriversInTeamById(Guid id)
        {
            try
            {
                var sql = @"SELECT COUNT(*) FROM Teams t
                            INNER JOIN Drivers d 
                            ON t.TeamId = d.TeamId WHERE t.TeamId = @Id";

                return await _connection.ExecuteScalarAsync<int>(sql, new { Id = id});
            }
            catch (Exception ex)
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
                await _connection.ExecuteAsync(query, team);
            }
            catch (Exception ex)
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
            catch (Exception ex)
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
            catch (Exception ex)
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

        public async Task UpdateTeamCountryAsync(Guid teamId, string country)
        {
            try
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
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
