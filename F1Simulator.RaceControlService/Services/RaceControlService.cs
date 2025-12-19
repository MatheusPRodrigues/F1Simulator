using F1Simulator.Models.DTOs.EngineeringService;
using F1Simulator.Models.DTOs.RaceControlService;
using F1Simulator.Models.DTOs.TeamManegementService.CarDTO;
using F1Simulator.RaceControlService.Messaging;
using F1Simulator.RaceControlService.Repositories.Interfaces;
using F1Simulator.RaceControlService.Services.Interfaces;
using System.Net;
using System.Text.Json;

namespace F1Simulator.RaceControlService.Services
{
    public class RaceControlService : IRaceControlService
    {
        private readonly IPublishService _messageService;
        private readonly IRaceControlRepository _raceControlRepository;
        private readonly ILogger<RaceControlService> _logger;
        private readonly HttpClient _engineeringClient;
        private readonly HttpClient _teamManagementClient;

        public RaceControlService(
            IPublishService messageService,
            IRaceControlRepository raceControlRepository,
            ILogger<RaceControlService> logger,
            IHttpClientFactory factory
        )
        {
            _messageService = messageService; 
            _raceControlRepository = raceControlRepository;
            _logger = logger;
            _engineeringClient = factory.CreateClient("EngineeringService");
            _teamManagementClient = factory.CreateClient("TeamManagementServicesDrivers");
        }

        public async Task<List<DriverGridResponseDTO>> ExecuteQualifierSectionAsync(string raceId)
        {
            try
            {
                var drivers = await _teamManagementClient.GetFromJsonAsync<List<DriverToRaceDTO>>("/drivers/race");
                var luck = Random.Shared.Next(1, 11);
                var driverProcessToGrid = new List<DriverGridResponseDTO>();

                foreach (var d in drivers)
                {
                    var request = new EngineersPutDTO
                    {
                        EngineerCaId = d.EnginneringAId,
                        EngineerCpId = d.EnginneringPId
                    };

                    var response = await _engineeringClient.PutAsJsonAsync($"car/{d.CarId}", request);
                    var processedResponse = JsonSerializer.Deserialize<CarUpdateDTO>(await response.Content.ReadAsStringAsync());

                    var newCa = processedResponse.Ca;
                    var newCp = processedResponse.Cp;

                    var newHandicap = d.Handicap - (d.DriverExp * 0.5);
                    await _teamManagementClient.PatchAsJsonAsync($"car/{d.CarId}/handicap", new { Handicap = newHandicap });

                    var dto = new DriverGridResponseDTO
                    {
                        DriverId = d.DriverId,
                        DriverName = d.DriverName,
                        Handicap = newHandicap,
                        TeamId = d.TeamId,
                        TeamName = d.TeamName,
                        CarId = d.CarId,
                        Ca = newCa,
                        Cp = newCp
                    };

                    dto.Pd = (dto.Ca * 0.4) + (dto.Cp * 0.4) - dto.Handicap + luck;
                    driverProcessToGrid.Add(dto);
                }

                driverProcessToGrid.Sort((a, b) => b.Pd.CompareTo(a.Pd));

                for (var i = 0; i < driverProcessToGrid.Count; i++)
                {
                    driverProcessToGrid[i].Position = i + 1;
                }

                return driverProcessToGrid;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while execute qualifier section: {ex}");
                throw;
            }
        }

        public async Task<List<DriverGridFinalRaceResponseDTO>> ExecuteRaceSectionAsync(string raceId)
        {
            try
            {
                const string PUBLISHQUEUE = "RaceFinishedEvent";
                int[] pontuationArray = { 25, 18, 15, 12, 10, 8, 6, 4, 2, 1 };
                var luck = Random.Shared.Next(1, 11);

                var responseTeamClient = await _teamManagementClient.GetAsync("/drivers/race");

                if (responseTeamClient.StatusCode == HttpStatusCode.NotFound)
                    throw new ArgumentException("Drivers not found to race");

                if (responseTeamClient.StatusCode == HttpStatusCode.InternalServerError)
                    throw new Exception();

                var drivers = await responseTeamClient.Content.ReadFromJsonAsync<List<DriverToRaceDTO>>();

                if (drivers is null)
                    throw new ArgumentException("Drivers not found to race");

                var driverProcessToGrid = new List<DriverGridFinalRaceResponseDTO>();

                foreach (var d in drivers)
                {
                    // recebe novos valores de Ca e Cp
                    var request = new EngineersPutDTO
                    {
                        EngineerCaId = d.EnginneringAId,
                        EngineerCpId = d.EnginneringPId
                    };

                    var responseEngineerClient = await _engineeringClient.PutAsJsonAsync($"car/{d.CarId}", request);
                    //TODO: caso a api retorne algo diferente de 200 prosseguir com os dados atuais

                    var processedResponse = JsonSerializer.Deserialize<CarUpdateDTO>(await responseEngineerClient.Content.ReadAsStringAsync());

                    var newCa = processedResponse.Ca;
                    var newCp = processedResponse.Cp;

                    var newHandicap = d.Handicap - (d.DriverExp * 0.5);
                    await _teamManagementClient.PatchAsJsonAsync($"car/{d.CarId}/handicap", new { Handicap = newHandicap });

                    var dto = new DriverGridFinalRaceResponseDTO
                    {
                        DriverId = d.DriverId,
                        DriverName = d.DriverName,
                        Handicap = newHandicap,
                        TeamId = d.TeamId,
                        TeamName = d.TeamName,
                        CarId = d.CarId,
                        Ca = newCa,
                        Cp = newCp
                    };

                    dto.Pd = (dto.Ca * 0.4) + (dto.Cp * 0.4) - dto.Handicap + luck;
                    driverProcessToGrid.Add(dto);
                }

                driverProcessToGrid.Sort((a, b) => b.Pd.CompareTo(a.Pd));

                for (var i = 0; i < driverProcessToGrid.Count; i++)
                {
                    if (i < 10)
                        driverProcessToGrid[i].Pontuation = pontuationArray[i];

                    driverProcessToGrid[i].Position = i + 1;
                }

                var processedList = ProcessDtoToPublish(driverProcessToGrid);
                await _messageService.Publish(processedList, PUBLISHQUEUE);

                return driverProcessToGrid;
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while execute race section: {ex}");
                throw;
            }
        }

        public async Task<List<DriverComparisonResponseDTO>> ExecuteTlOneSectionAsync(string raceId)
        {
            try
            {
                var drivers = await _teamManagementClient.GetFromJsonAsync<List<DriverToRaceDTO>>("/drivers/race");

                var response = await ProcessingDriversComparison(drivers);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while execute tl one section: {ex}");
                throw;
            }
        }

        public async Task<List<DriverComparisonResponseDTO>> ExecuteTlThreeSectionAsync(string raceId)
        {
            try
            {
                var drivers = await _teamManagementClient.GetFromJsonAsync<List<DriverToRaceDTO>>("/drivers/race");

                var response = await ProcessingDriversComparison(drivers);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while execute tl two section: {ex}");
                throw;
            }
        }

        public async Task<List<DriverComparisonResponseDTO>> ExecuteTlTwoSectionAsync(string raceId)
        {
            try
            {
                var drivers = await _teamManagementClient.GetFromJsonAsync<List<DriverToRaceDTO>>("/drivers/race");

                var response = await ProcessingDriversComparison(drivers);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while execute tl three section: {ex}");
                throw;
            }
        }

        private async Task<List<DriverComparisonResponseDTO>> ProcessingDriversComparison(List<DriverToRaceDTO> drivers)
        {
            var driversComparison = new List<DriverComparisonResponseDTO>();
            foreach (var d in drivers)
            {
                var request = new EngineersPutDTO
                {
                    EngineerCaId = d.EnginneringAId,
                    EngineerCpId = d.EnginneringPId
                };

                var responseEngineeringAPI = await _engineeringClient.PutAsJsonAsync($"car/{d.CarId}", request);
                var processedResponse = JsonSerializer.Deserialize<CarUpdateDTO>(await responseEngineeringAPI.Content.ReadAsStringAsync());

                var newCa = processedResponse.Ca;
                var newCp = processedResponse.Cp;

                var newHandicap = d.Handicap - (d.DriverExp * 0.5);
                await _teamManagementClient.PatchAsJsonAsync($"car/{d.CarId}/handicap", new { Handicap = newHandicap });

                var comparison = new DriverComparisonResponseDTO
                {
                    OlderStats = new DriverAndCarStatsDTO
                    {
                        DriverId = d.DriverId,
                        DriverName = d.DriverName,
                        Handicap = d.Handicap,
                        CarId = d.CarId,
                        Ca = d.Ca,
                        Cp = d.Cp
                    },
                    NewStats = new DriverAndCarStatsDTO
                    {
                        DriverId = d.DriverId,
                        DriverName = d.DriverName,
                        Handicap = newHandicap,
                        CarId = d.CarId,
                        Ca = newCa,
                        Cp = newCp
                    }
                };
                driversComparison.Add(comparison);
            }

            return driversComparison;
        }

        private List<DriverToPublishDTO> ProcessDtoToPublish(List<DriverGridFinalRaceResponseDTO> dto)
        {
            var publishList = new List<DriverToPublishDTO>();

            foreach (var d in dto)
            {
                publishList.Add(new DriverToPublishDTO
                {
                    DriverId = d.DriverId,
                    DriverName = d.DriverName,
                    TeamId = d.TeamId,
                    TeamName = d.TeamName,
                    Pontuation = d.Pontuation
                });
            }

            return publishList;
        }
    }
}
