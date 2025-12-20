using F1Simulator.Models.DTOs.RaceControlService;
using F1Simulator.Models.DTOs.TeamManegementService.CarDTO;
using F1Simulator.Models.DTOs.TeamManegementService.DriverDTO;
using F1Simulator.Models.Models.TeamManegement;
using F1Simulator.TeamManagementService.Repositories.Interfaces;
using F1Simulator.TeamManagementService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Runtime.CompilerServices;

namespace F1Simulator.TeamManagementService.Services
{
    public class DriverService : IDriverService
    {
        private readonly Random _random = Random.Shared;
        private readonly IDriverRepository _driverRepository;
        private readonly ICarService _carService;
        private readonly ITeamService _teamService;

        public DriverService(IDriverRepository driverRepository, ICarService carService, ITeamService teamService)
        {
            _driverRepository = driverRepository;
            _carService = carService;
            _teamService = teamService;
        }

        public async Task<DriverResponseDTO> CreateDriverAsync(DriverRequestDTO driverRequest)
        {
            try
            {

                if (await _driverRepository.GetDriverByNumberAsync(driverRequest.DriverNumber) is not null)
                    throw new InvalidOperationException("There is already a pilot with that number.");

                if (await _carService.GetCountCarByIdCar(driverRequest.CarId) == 1)
                    throw new InvalidOperationException("This car is already linked to a driver.");

                var team = await _teamService.GetTeamByIdAsync(driverRequest.TeamId.ToString());

                if (team is null)
                    throw new KeyNotFoundException("Team not found.");

                if(await _teamService.GetDriversInTeamById(driverRequest.TeamId) >= 2)
                    throw new InvalidOperationException("The Team is already with two drivers");
                var experienceDriver = Math.Round((5.0 * _random.NextDouble()), 3);

                double handicap = 50.0 + (_random.NextDouble() * 50.0);
                var driver = new Driver
                (
                    driverRequest.DriverNumber,
                    driverRequest.TeamId,
                    driverRequest.CarId,
                    driverRequest.FirstName,
                    driverRequest.FullName,
                    driverRequest.WeightKg,
                    experienceDriver,
                    handicap
                );

                var model = team.NameAcronym + driver.DriverNumber;

                var driverStorage = await _driverRepository.CreateDriverAsync(driver);
                var updateCarModel = new CarModelUpdateDTO { Model = model };
                var updateModelCar = _carService.UpdateCarModelAsync(updateCarModel, driver.CarId.ToString());
                return driverStorage;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<DriverResponseDTO>> GetDriversAsync()
        {
            try
            {
                return await _driverRepository.GetAllDriversAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<DriverResponseDTO> GetDriverByIdAsync(Guid id)
        {
            try
            {
                return await _driverRepository.GetDriverByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<DriverToRaceDTO>> GetDriversToRaceAsync()
        {
            try
            {
                return await _driverRepository.GetDriversToRace();
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateDriverAsync(Guid id ,UpdateRequestDriverDTO driverRequest)
        {
            try
            {
                await _driverRepository.UpdateDriverAsync(id, driverRequest);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> GetAllDriversCount()
        {
            try
            {
                return await _driverRepository.GetAllDriversCount();
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
