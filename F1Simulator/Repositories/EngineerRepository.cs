using Dapper;
using F1Simulator.Models.DTOs.TeamManegementService.EngineerDTO;
using F1Simulator.Models.Models.TeamManegement;
using F1Simulator.TeamManagementService.Repositories.Interfaces;
using F1Simulator.Utils.DatabaseConnectionFactory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace F1Simulator.TeamManagementService.Repositories
{
    public class EngineerRepository : IEngineerRepository
    {
        private readonly SqlConnection _connection;
        public EngineerRepository(IDatabaseConnection<SqlConnection> connection)
        {
            _connection = connection.Connect();
        }

        public async Task<EngineerResponseDTO> CreateEngineerAsync(Engineer engineer)
        {
            try
            {
                var sql = "INSERT INTO Engineers(EngineerId, TeamId, CarId, FirstName, LastName, Specialization, IsActive) VALUES (@EngineerId, @TeamId, @CarId, @FirstName, @LastName, @Specialization, @IsActive) SELECT SCOPE_IDENTITY() ";

                var executeReturnId = await _connection.ExecuteScalarAsync(sql, new { 
                    EngineerId = engineer.EngineerId,
                    TeamId = engineer.TeamId, 
                    CarId = engineer.CarId, 
                    FirstName = engineer.FirstName,
                    LastName = engineer.FullName, 
                    Specialization = engineer.EngineerSpecialization.ToString(), 
                    IsActive = engineer.IsActive 
                });

                var engineerResponse = new EngineerResponseDTO { EngineerId = Guid.Parse(executeReturnId.ToString()), CarId = engineer.CarId, EngineerSpecialization = engineer.EngineerSpecialization, ExperienceFactor = engineer.ExperienceFactor, FirstName = engineer.FirstName, FullName = engineer.FullName, IsActive = engineer.IsActive, TeamId = engineer.TeamId };
                return engineerResponse;
            } catch(SqlException ex)
            {
                throw new Exception(ex.Message);
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<EngineerResponseDTO>> GetAllEnginnersAsync()
        {
            try
            {
                var sql = @"SELECT EngineerId, TeamId, CarId, FirstName, LastName, Specialization, IsActive FROM Engineers";
                var engineers = await _connection.QueryAsync<EngineerResponseDTO>(sql);

                return engineers.ToList();
            } catch(SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EngineerResponseDTO> GetEngineerById(Guid id)
        {
            try
            {
                var sql = @"SELECT EngineerId, TeamId, CarId, FirstName, LastName, Specialization, IsActive FROM Engineers WHERE EngineerId = @EngineerId";

                return await _connection.QueryFirstOrDefaultAsync<EngineerResponseDTO>(sql, new { EngineerId = id});
            } catch(SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateActiveEngineer(EngineerUpdateRequestDTO engineerUpdateRequestDTO)
        {
            try
            {

            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
