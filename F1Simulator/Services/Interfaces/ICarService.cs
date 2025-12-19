using F1Simulator.Models.DTOs.TeamManegementService.CarDTO;

namespace F1Simulator.TeamManagementService.Services.Interfaces
{
    public interface ICarService
    {
        Task CreateCarAsync(CarRequestDTO car);

        Task<List<CarResponseDTO>> GetAllCarAsync();

        Task<CarResponseDTO> GetCarByIdAsync(string id);
        Task<int> GetCountCarByIdCar(Guid carId);

        Task UpdateCarCoefficientsAsync(CarUpdateDTO carUpdate, string carId);

        Task UpdateCarModelAsync(CarModelUpdateDTO carModelUpdate, string carId);
    }
}
