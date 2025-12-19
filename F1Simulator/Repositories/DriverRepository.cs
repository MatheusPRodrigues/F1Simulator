using F1Simulator.Models.DTOs.TeamManegementService.DriverDTO;
using F1Simulator.Models.Models.TeamManegement;
using F1Simulator.TeamManagementService.Repositories.Interfaces;
using F1Simulator.Utils.DatabaseConnectionFactory;
using Microsoft.Data.SqlClient;

namespace F1Simulator.TeamManagementService.Repositories
{
    public class DriverRepository : IDriverRepository
    {
        private readonly SqlConnection _connection;
        public DriverRepository(IDatabaseConnection<SqlConnection> connection)
        {
            _connection = connection.Connect();
        }

        public Task<DriverResponseDTO> CreateDriverAsync(Driver driver)
        {
            try
            {
                var sql = @"INSERT INTO Drivers(DriverId, DriverNumber, TeamId, CarId, FirstName, FullName, NameAcronym, WeightKg, HandiCap, IsActive)";
            } catch(SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<List<DriverResponseDTO>> GetAllDriversAsync()
        {
            throw new NotImplementedException();
        }

        public Task<DriverResponseDTO> GetDriverByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<DriverResponseDTO> UpdateDriverAsync(Driver driver)
        {
            throw new NotImplementedException();
        }
    }
}
