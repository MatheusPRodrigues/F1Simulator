using Dapper;
using F1Simulator.Models.DTOs.TeamManegementService.BossDTO;
using F1Simulator.Models.DTOs.TeamManegementService.TeamDTO;
using F1Simulator.Models.Models.TeamManegementService;
using F1Simulator.TeamManagementService.Repositories.Interfaces;
using F1Simulator.Utils.DatabaseConnectionFactory;
using Microsoft.Data.SqlClient;

namespace F1Simulator.TeamManagementService.Repositories
{
    public class BossRepository : IBossRepository
    {
        private readonly SqlConnection _connection;
        public BossRepository(IDatabaseConnection<SqlConnection> connection)
        {
            _connection = connection.Connect();
        }

        public async Task<int> GetBossByTeamCountAsync(Guid teamId)
        {
            try
            {
                var sql = @"SELECT COUNT(*) FROM Boss
                            WHERE TeamId = @TeamId";

                return await _connection.ExecuteScalarAsync<int>(sql, new { TeamId = teamId});
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task CreateBossAsync(Boss boss)
        {
            try
            {
                var sql = @"INSERT INTO Boss (BossId, TeamId, FirstName, LastName, Age)
                        VALUES (@BossId, @TeamId, @FirstName, @LastName, @Age)";

                await _connection.ExecuteAsync(sql, boss);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<BossResponseDTO>> GetAllBossesAsync()
        {
            try
            {
                var sql = @"SELECT BossId, TeamId, FirstName, LastName, Age
                        FROM Boss";

                return await _connection.QueryAsync<BossResponseDTO>(sql);
            }
            catch(SqlException ex)
            {
                throw new Exception("Error occurred while getting all bosses", ex);
            }
        }
        public async Task<IEnumerable<BossWithTeamDTO>> GetBossesWithTeamAsync()
        {
            try
            {
                var sql = @"SELECT b.BossId, b.FirstName, b.LastName, b.Age,
                            t.TeamId, t.Name, t.NameAcronym, t.Country 
                            FROM Boss b
                            INNER JOIN Teams t ON b.TeamId = t.TeamId";

                var result = await _connection.QueryAsync<BossWithTeamDTO, TeamResponseDTO, BossWithTeamDTO>
                (sql, (boss, team) =>
                {
                    boss.Team = team;
                    return boss;
                },
                splitOn: "TeamId"
                );

                return result;
            }
            catch(SqlException ex)
            {
                throw new Exception("Error occurred while getting all bosses with their teams!", ex);
            }
        }

        public async Task<int> GetAllBossesCountAsync()
        {
            try
            {
                var sql = @"SELECT COUNT(*) FROM Boss";

                return await _connection.ExecuteScalarAsync<int>(sql);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
