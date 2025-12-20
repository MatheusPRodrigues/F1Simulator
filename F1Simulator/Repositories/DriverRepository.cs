using Dapper;
using F1Simulator.Models.DTOs.RaceControlService;
using F1Simulator.Models.DTOs.TeamManegementService.DriverDTO;
using F1Simulator.Models.Models.TeamManegement;
using F1Simulator.TeamManagementService.Repositories.Interfaces;
using F1Simulator.Utils.DatabaseConnectionFactory;
using Microsoft.Data.SqlClient;
using ZstdSharp.Unsafe;

namespace F1Simulator.TeamManagementService.Repositories
{
    public class DriverRepository : IDriverRepository
    {
        private readonly SqlConnection _connection;
        public DriverRepository(IDatabaseConnection<SqlConnection> connection)
        {
            _connection = connection.Connect();
        }

        public async Task<DriverResponseDTO> CreateDriverAsync(Driver driver)
        {
            try
            {
                var sql = @"INSERT INTO Drivers(DriverId, DriverNumber, TeamId, CarId, FirstName, FullName, WeightKg, HandiCap, ExperienceFactor) 
                            VALUES (@DriverId, @DriverNumber, @TeamId, @CarId, @FirstName, @FullName, @WeightKg, @HandiCap, @ExperienceFactor)";
                await _connection.ExecuteAsync(sql, 
                    new {
                        DriverId = driver.DriverId, 
                        DriverNumber = driver.DriverNumber, 
                        TeamId = driver.TeamId,
                        CarId = driver.CarId,
                        FirstName = driver.FirstName,
                        FullName = driver.FullName,
                        WeightKg = driver.WeightKg,
                        HandiCap = driver.HandiCap,
                        ExperienceFactor = driver.ExperienceFactor
                    });

                var driverResponse = new DriverResponseDTO
                {
                    DriverId = driver.DriverId,
                    CarId = driver.CarId,
                    DriverNumber = driver.DriverNumber,
                    FirstName = driver.FirstName,
                    FullName = driver.FullName,
                    HandiCap = driver.HandiCap,
                    TeamId = driver.TeamId,
                    WeightKg = driver.WeightKg,
                    ExperienceFactor = driver.ExperienceFactor
                };
                return driverResponse;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<DriverResponseDTO>> GetAllDriversAsync()
        {
            try
            {
                var sql = @"SELECT DriverId, DriverNumber, TeamId, CarId, FirstName, FullName, WeightKg, HandiCap, ExperienceFactor
                            FROM Drivers";
                var drivers = await _connection.QueryAsync<DriverResponseDTO>(sql);
                return drivers.ToList();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> GetAllDriversCount()
        {
            try
            {
                var sql = @"SELECT COUNT(*) FROM Drivers";
                return await _connection.ExecuteScalarAsync<int>(sql);
            } catch(SqlException ex)
            {
                throw new Exception(ex.Message);
            } 
        }

        public async Task<DriverResponseDTO> GetDriverByIdAsync(Guid id)
        {
            try
            {
                var sql = @"SELECT DriverId, DriverNumber, TeamId, CarId, FirstName, FullName, WeightKg, HandiCap FROM Drivers WHERE DriverId = @DriverId";
                var driver = await _connection.QueryFirstOrDefaultAsync<DriverResponseDTO>(sql, new { DriverId = id });
                return driver;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<DriverResponseDTO> GetDriverByNumberAsync(int number)
        {
            try
            {
                var sql = @"SELECT DriverId, DriverNumber, TeamId, CarId, FirstName, FullName, WeightKg, HandiCap FROM Drivers WHERE DriverNumber = @DriverNumber";
                var driver = await _connection.QueryFirstOrDefaultAsync<DriverResponseDTO>(sql, new { DriverNumber = number });
                return driver;
            } catch(SqlException ex)
            {
                throw new Exception(ex.Message);
            } 
        }

        public async Task<List<DriverToRaceDTO>> GetDriversToRace()
        {
            try
            {
                var sql = @"SELECT
                            d.DriverId AS DriverId,
                            d.FullName AS DriverName,
                            d.HandiCap AS Handicap,
                            d.ExperienceFactor AS DriverExp,
                            t.TeamId AS TeamId,
                            t.[Name] AS TeamName,
                            eCa.EngineerId AS EnginneringAId,
                            eCp.EngineerId AS EnginneringPId,
                            c.CarId AS CarId,
                            c.Ca AS Ca,
                            c.Cp AS Cp
                        FROM [Drivers] d
                        INNER JOIN Teams t
                        ON t.TeamId = d.TeamId
                        INNER JOIN Cars c
                        ON c.CarId = d.CarId
                        INNER JOIN Engineers eCa
                            ON eCa.CarId = c.CarId
                           AND eCa.Specialization = 'Ca'
                        INNER JOIN Engineers eCp
                            ON eCp.CarId = c.CarId
                           AND eCp.Specialization = 'Cp';";

                var driversToRace =  await _connection.QueryAsync<DriverToRaceDTO>(sql);
                return driversToRace.ToList();
            } catch(SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateDriverAsync(Guid id, UpdateRequestDriverDTO driver)
        {
            try
            {
                var sql = @"UPDATE Drivers SET HandiCap = @HandiCap WHERE Drivers.DriverId = @DriverId";
                await _connection.ExecuteAsync(sql, new { HandiCap = driver.Handicap, DriverId = id });
            } catch(SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
