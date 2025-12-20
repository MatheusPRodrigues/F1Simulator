using F1Simulator.Models.DTOs.TeamManegementService.CarDTO;
using F1Simulator.Models.Models.TeamManegement;

namespace F1Simulator.TeamManagementService.Repositories.Interfaces
{
    public interface ICarRepository
    {
        Task CreateCarAsync(Car car);

        Task<List<CarResponseDTO>> GetAllCarAsync();

        Task<CarResponseDTO> GetCarByIdAsync(string id);

        Task UpdateCarCoefficientsAsync(CarUpdateDTO carUpdate, string carId);

        Task UpdateCarModelAsync(CarModelUpdateDTO carUpdate, string carId);

        Task<int> GetCountCarsByIdTeam(string teamId);
        Task<int> GetCountCarsByIdCar(Guid carId);
        Task<int> GetAllCarsCountAsync();

    }
}
