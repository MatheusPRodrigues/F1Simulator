using F1Simulator.Models.DTOs.TeamManegementService.CarDTO;
using F1Simulator.Models.DTOs.TeamManegementService.TeamDTO;
using F1Simulator.Models.Models.TeamManegement;

namespace F1Simulator.TeamManagementService.Repositories.Interfaces
{
    public interface ICarRepository
    {
        Task CreateCarAsync(Car car);

        Task<List<CarResponseDTO>> GetAllCarAsync(string id);

        Task<CarResponseDTO> GetCarByIdAsync(string id);

        Task UpdateCarCoefficientsAsync(CarUpdateDTO carUpdate, string carId);

        Task UpdateCarModelAsync(CarModelUpdateDTO carUpdate, string carId);

    }
}
