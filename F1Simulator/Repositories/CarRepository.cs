using Dapper;
using F1Simulator.Models.DTOs.TeamManegementService.CarDTO;
using F1Simulator.Models.DTOs.TeamManegementService.TeamDTO;
using F1Simulator.Models.Models.TeamManegement;
using F1Simulator.TeamManagementService.Data;
using F1Simulator.TeamManagementService.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace F1Simulator.TeamManagementService.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly SqlConnection _connection;
        public CarRepository(TeamManagementServiceConnection connection)
        {
            _connection = connection.GetConnection();
        }


        public async Task CreateCarAsync(Car car)
        {
            try
            {
                var sql = @"INSERT INTO [Cars] 
                            ([CarId], [TeamId], [Model], [WeightKg], [Speed], [Ca], [Cp]) 
                            VALUES 
                            (@CarId, @TeamId, @Model, @WeightKg, @Speed, @Ca, C@p)";

                await _connection.ExecuteAsync(sql, new {car.CarId, car.TeamId,car.Model,
                                                        car.WeightKg, car.Speed, car.Ca, car.Cp});
            }
            catch (SqlException ex)
            {
                throw new Exception("Error querying the database.", ex);
            }
        }


        public async Task<List<CarResponseDTO>> GetAllCarAsync(string id)
        {
            try
            {
                var sql = @"SELECT 
                            [CarId], [TeamId], [Model], [WeightKg], [Speed], [Ca], [Cp] 
                            FROM [Cars]";


                return (await _connection.QueryAsync<CarResponseDTO>(sql)).ToList();

            }
            catch (Exception ex)
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


        public async Task<TeamResponseDTO> GetTeamByIdAsync(string teamId)
        {
            try
            {
                var sql = @"SELECT 
                            [TeamId], [Name], [NameAcronym], [Country]
                            FROM [Teams] 
                            WHERE [TeamId] = @TeamId";

                return await _connection.QueryFirstOrDefaultAsync<TeamResponseDTO>(sql, new { TeamId = teamId });
            }
            catch (SqlException ex)
            {
                throw new Exception("Error querying the database.", ex);
            }
        }

    }
}
