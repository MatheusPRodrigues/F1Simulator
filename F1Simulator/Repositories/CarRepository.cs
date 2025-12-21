using Dapper;
using F1Simulator.Models.DTOs.TeamManegementService.CarDTO;
using F1Simulator.Models.Models.TeamManegement;
using F1Simulator.TeamManagementService.Repositories.Interfaces;
using F1Simulator.Utils.DatabaseConnectionFactory;
using Microsoft.Data.SqlClient;

namespace F1Simulator.TeamManagementService.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly SqlConnection _connection;

        public CarRepository(IDatabaseConnection<SqlConnection> connection)
        {
            _connection = connection.Connect();
        }

        public async Task CreateCarAsync(Car car)
        {
            try
            {
                var sql = @"INSERT INTO [Cars] 
                            ([CarId], [TeamId], [Model], [WeightKg], [Speed], [Ca], [Cp]) 
                            VALUES 
                            (@CarId, @TeamId, @Model, @WeightKg, @Speed, @Ca, @Cp)";

                await _connection.ExecuteAsync(sql, new {car.CarId, car.TeamId,car.Model,
                                                        car.WeightKg, car.Speed, car.Ca, car.Cp});
            }
            catch (SqlException ex)
            {
                throw new Exception("Error querying the database.", ex);
            }
        }


        public async Task<List<CarResponseDTO>> GetAllCarAsync()
        {
            try
            {
                var sql = @"SELECT 
                            [CarId], [TeamId], [Model], [WeightKg], [Speed], [Ca], [Cp] 
                            FROM [Cars]";

                return (await _connection.QueryAsync<CarResponseDTO>(sql)).ToList();

            }
            catch (SqlException ex)
            {
                throw new Exception("Error querying the database.", ex);
            }
        }


        public async Task<CarResponseDTO> GetCarByIdAsync(string carId)
        {
            try
            {
                var sql = @"SELECT 
                            [CarId], [TeamId], [Model], [WeightKg], [Speed], [Ca], [Cp]
                            FROM [Cars] 
                            WHERE [CarId] = @CarId";

                return await _connection.QueryFirstOrDefaultAsync<CarResponseDTO>(sql, new { CarId = carId});
            }
            catch (SqlException ex)
            {
                throw new Exception("Error querying the database.", ex);
            }
        }


        public async Task UpdateCarCoefficientsAsync(CarUpdateDTO carUpdate, string carId)
        {
            try
            {
                var sql = @"UPDATE [Cars] 
                            SET Ca = @Ca, Cp = @Cp 
                            WHERE [CarId] = @CarId";

                await _connection.ExecuteAsync(sql, new { carUpdate.Ca, carUpdate.Cp, CarId = carId });
            }
            catch (SqlException ex)
            {
                throw new Exception("Error querying the database.", ex);
            }
        }


        public async Task UpdateCarModelAsync(CarModelUpdateDTO carModelUpdate, string carId)
        {
            try
            {
                var sql = @"UPDATE [Cars] 
                            SET Model = @Model 
                            WHERE [CarId] = @CarId";

                await _connection.ExecuteAsync(sql, new { carModelUpdate.Model, CarId = carId });
            }
            catch (SqlException ex)
            {
                throw new Exception("Error querying the database.", ex);
            }
        }


        public async Task<int> GetCountCarsByIdTeamAsync(string teamId)
        {
            try
            {
                var sql = @"SELECT COUNT (*) FROM Cars c
                            INNER JOIN Teams t
                            ON c.TeamId = t.TeamId
                            WHERE c.TeamId = @TeamId";

                return await _connection.QueryFirstOrDefaultAsync<int>(sql, new { teamId });
            }
            catch (SqlException ex)
            {
                throw new Exception("Error querying the database.", ex);
            }
        }

        public async Task<int> GetCountCarsByIdCarAsync(string carId)
        {
            try
            {
                var sql = @"SELECT COUNT(*)
                            FROM Cars c
                            INNER JOIN Drivers d ON d.CarId = c.CarId
                            WHERE c.CarId = @CarId;
                            ";

                return await _connection.QueryFirstOrDefaultAsync<int>(sql, new { CarId = carId });
            }
            catch (SqlException ex)
            {
                throw new Exception("Error querying the database.", ex);
            }
        }

        public async Task<int> GetAllCarsCountAsync()
        {
            try
            {
                var sql = @"SELECT COUNT(*) FROM Cars";

                return await _connection.ExecuteScalarAsync<int>(sql);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
