using Microsoft.Data.SqlClient;

namespace F1Simulator.TeamManagementService.Data
{
    public class TeamManagementServiceConnection
    {
        private readonly string _connectionString;
        public TeamManagementServiceConnection(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlServer");
        }
        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
