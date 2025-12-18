using Dapper;
using F1Simulator.Models.Models.TeamManegementService;
using F1Simulator.Utils.DatabaseConnectionFactory;
using Microsoft.Data.SqlClient;

namespace F1Simulator.TeamManagementService.Repositories
{
    public class BossRepository
    {
        private readonly SqlConnection _connection;
        public BossRepository(IDatabaseConnection<SqlConnection> connection)
        {
            _connection = connection.Connect();
        }

        public async Task<int> GetBossByTeamCountAsync(Guid teamId)
        {
            var sql = @"SELECT COUNT(*) FROM Boss
                        WHERE TeamId = @TeamId";

            return await _connection.ExecuteScalarAsync<int>(sql, new { TeamId = teamId});
        }

        public async Task CreateBossAsync(Boss boss)
        {
            var sql = @"INSERT INTO Boss (BossId, TeamId, FirstName, FullName, Age, IsActive)
                        VALUES (@BossId, @TeamId, @FirstName, @LastName, @Age, @IsActive)";

            await _connection.ExecuteAsync(sql, boss);
        }
    }
}
