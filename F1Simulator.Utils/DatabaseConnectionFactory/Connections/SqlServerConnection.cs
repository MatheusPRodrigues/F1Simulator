using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace F1Simulator.Utils.DatabaseConnectionFactory.Connections
{
    public class SqlServerConnection : IDatabaseConnection<SqlConnection>
    {
        private readonly string _connectionString;

        public SqlServerConnection(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlServer");
        }

        public SqlConnection Connect()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
