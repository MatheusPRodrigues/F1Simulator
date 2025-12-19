using F1Simulator.Models.DTOs.CompetitionService.Response;
using F1Simulator.Models.DTOs.EngineeringService;
using F1Simulator.Models.DTOs.RaceControlService;
using F1Simulator.Models.DTOs.TeamManegementService.CarDTO;
using F1Simulator.Models.Models.RaceControlService;
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
        private readonly HttpClient _competitionClient;

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
            _competitionClient = factory.CreateClient("CompetitionService");
        }

        public async Task<List<DriverGridResponseDTO>> ExecuteQualifierSectionAsync()
        {
            try
            {
                var responseCompetitionClient = await _competitionClient.GetAsync("races/inprogress");

                if (responseCompetitionClient.StatusCode == HttpStatusCode.NotFound)
                    throw new KeyNotFoundException(await responseCompetitionClient.Content.ReadAsStringAsync());

                if (responseCompetitionClient.StatusCode == HttpStatusCode.InternalServerError)
                    throw new Exception();

                var race = await responseCompetitionClient.Content.ReadFromJsonAsync<RaceWithCircuitResponseDTO>();

                if (!race.T1 || !race.T2 || !race.T3)
                    throw new ArgumentException("This section cannot be started yet");

                if (race.Qualifier)
                    throw new ArgumentException("This section has already been completed");

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

                responseCompetitionClient = await _competitionClient.SendAsync(new HttpRequestMessage(HttpMethod.Patch, "races/qualifier"));

                if (responseCompetitionClient.StatusCode == HttpStatusCode.NotFound)
                    throw new ArgumentException("This section cannot be started yet");

                if (responseCompetitionClient.StatusCode == HttpStatusCode.InternalServerError)
                    throw new Exception();

                var raceControlToPersist = CreateRaceControlModelObject(race, driverProcessToGrid);
                await _raceControlRepository.InsertRaceControlRegisterAsync(raceControlToPersist);

                return driverProcessToGrid;
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while execute qualifier section: {ex}");
                throw;
            }
        }

        public async Task<List<DriverGridFinalRaceResponseDTO>> ExecuteRaceSectionAsync()
        {
            try
            {
                var responseCompetitionClient = await _competitionClient.GetAsync("races/inprogress");

                if (responseCompetitionClient.StatusCode == HttpStatusCode.NotFound)
                    throw new KeyNotFoundException(await responseCompetitionClient.Content.ReadAsStringAsync());

                if (responseCompetitionClient.StatusCode == HttpStatusCode.InternalServerError)
                    throw new Exception();

                var race = await responseCompetitionClient.Content.ReadFromJsonAsync<RaceWithCircuitResponseDTO>();

                if (!race.T1 || !race.T2 || !race.T3 || !race.Qualifier)
                    throw new ArgumentException("This section cannot be started yet");

                if (race.RaceFinal)
                    throw new ArgumentException("This section has already been completed");

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

                var gridQualifier = await _raceControlRepository.GetDriverQualiersByRaceId(race.Id);

                var driversWithPositionQualifierGrid = new List<DriverToRaceWithPositionDTO>();
                for (var i = 0; i < drivers.Count; i++)
                {
                    if (drivers[i].DriverId == gridQualifier[i].DriverId.ToString())
                    {
                        var driverPosition = new DriverToRaceWithPositionDTO
                        {
                            DriverId = drivers[i].DriverId,
                            DriverName = drivers[i].DriverName,
                            Handicap = drivers[i].Handicap,
                            TeamId = drivers[i].TeamId,
                            TeamName = drivers[i].TeamName,
                            EnginneringAId = drivers[i].EnginneringAId,
                            EnginneringPId = drivers[i].EnginneringPId,
                            CarId = drivers[i].CarId,
                            Ca = drivers[i].Ca,
                            Cp = drivers[i].Cp,
                            Position = gridQualifier[i].Position
                        };
                        driversWithPositionQualifierGrid.Add(driverPosition);
                    }
                }

                driversWithPositionQualifierGrid.Sort((a, b) => a.Position.CompareTo(b.Position));

                var driverProcessToGrid = new List<DriverGridFinalRaceResponseDTO>();
                foreach (var d in driversWithPositionQualifierGrid)
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

                responseCompetitionClient = await _competitionClient.SendAsync(new HttpRequestMessage(HttpMethod.Patch, "races/race"));

                if (responseCompetitionClient.StatusCode == HttpStatusCode.NotFound)
                    throw new ArgumentException("This section cannot be started yet");

                if (responseCompetitionClient.StatusCode == HttpStatusCode.InternalServerError)
                    throw new Exception();

                var processedList = ProcessDtoToPublish(driverProcessToGrid);
                await _messageService.Publish(processedList, PUBLISHQUEUE);

                var raceToReplace = await AddGridFinalRaceInRaceControl(driverProcessToGrid, race.Id);
                await _raceControlRepository.ReplaceDriverRaceAsync(raceToReplace);

                return driverProcessToGrid;
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while execute race section: {ex}");
                throw;
            }
        }

        public async Task<List<DriverComparisonResponseDTO>> ExecuteTlOneSectionAsync()
        {
            try
            {
                var responseCompetitionClient = await _competitionClient.GetAsync("races/inprogress");

                if (responseCompetitionClient.StatusCode == HttpStatusCode.NotFound)
                    throw new KeyNotFoundException(await responseCompetitionClient.Content.ReadAsStringAsync());

                if (responseCompetitionClient.StatusCode == HttpStatusCode.InternalServerError)
                    throw new Exception();

                var race = await responseCompetitionClient.Content.ReadFromJsonAsync<RaceWithCircuitResponseDTO>();

                if (race.T1)
                    throw new ArgumentException("This section has already been completed");

                var drivers = await _teamManagementClient.GetFromJsonAsync<List<DriverToRaceDTO>>("/drivers/race");

                var response = await ProcessingDriversComparison(drivers);

                responseCompetitionClient = await _competitionClient.SendAsync(new HttpRequestMessage(HttpMethod.Patch, "races/t1"));

                if (responseCompetitionClient.StatusCode == HttpStatusCode.NotFound)
                    throw new ArgumentException("This section cannot be started yet");

                if (responseCompetitionClient.StatusCode == HttpStatusCode.InternalServerError)
                    throw new Exception();

                return response;
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while execute tl one section: {ex}");
                throw;
            }
        }

        public async Task<List<DriverComparisonResponseDTO>> ExecuteTlThreeSectionAsync()
        {
            try
            {
                var responseCompetitionClient = await _competitionClient.GetAsync("races/inprogress");

                if (responseCompetitionClient.StatusCode == HttpStatusCode.NotFound)
                    throw new KeyNotFoundException(await responseCompetitionClient.Content.ReadAsStringAsync());

                if (responseCompetitionClient.StatusCode == HttpStatusCode.InternalServerError)
                    throw new Exception();

                var race = await responseCompetitionClient.Content.ReadFromJsonAsync<RaceWithCircuitResponseDTO>();

                if (!race.T1 || !race.T2)
                    throw new ArgumentException("This section cannot be started yet");

                if (race.T3)
                    throw new ArgumentException("This section has already been completed.");

                var drivers = await _teamManagementClient.GetFromJsonAsync<List<DriverToRaceDTO>>("/drivers/race");

                var response = await ProcessingDriversComparison(drivers);

                responseCompetitionClient = await _competitionClient.SendAsync(new HttpRequestMessage(HttpMethod.Patch, "races/t3"));

                if (responseCompetitionClient.StatusCode == HttpStatusCode.NotFound)
                    throw new ArgumentException("This section cannot be started yet");

                if (responseCompetitionClient.StatusCode == HttpStatusCode.InternalServerError)
                    throw new Exception();

                return response;
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while execute tl two section: {ex}");
                throw;
            }
        }

        public async Task<List<DriverComparisonResponseDTO>> ExecuteTlTwoSectionAsync()
        {
            try
            {
                var responseCompetitionClient = await _competitionClient.GetAsync("races/inprogress");

                if (responseCompetitionClient.StatusCode == HttpStatusCode.NotFound)
                    throw new KeyNotFoundException(await responseCompetitionClient.Content.ReadAsStringAsync());

                if (responseCompetitionClient.StatusCode == HttpStatusCode.InternalServerError)
                    throw new Exception();

                var race = await responseCompetitionClient.Content.ReadFromJsonAsync<RaceWithCircuitResponseDTO>();

                if (!race.T1)
                    throw new ArgumentException("This section cannot be started yet");

                if (race.T2)
                    throw new ArgumentException("This section has already been completed.");

                var drivers = await _teamManagementClient.GetFromJsonAsync<List<DriverToRaceDTO>>("/drivers/race");

                var response = await ProcessingDriversComparison(drivers);

                responseCompetitionClient = await _competitionClient.SendAsync(new HttpRequestMessage(HttpMethod.Patch, "races/t2"));

                if (responseCompetitionClient.StatusCode == HttpStatusCode.NotFound)
                    throw new ArgumentException("This section cannot be started yet");

                if (responseCompetitionClient.StatusCode == HttpStatusCode.InternalServerError)
                    throw new Exception();

                return response;
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while execute tl three section: {ex}");
                throw;
            }
        }

        private async Task<List<DriverComparisonResponseDTO>> ProcessingDriversComparison(List<DriverToRaceDTO> drivers)
        {
            try
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
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while execute race section: {ex}");
                throw;
            }
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

        private RaceControl CreateRaceControlModelObject(RaceWithCircuitResponseDTO raceDto, List<DriverGridResponseDTO> driverGrids)
        {
            var driversQualifiers = new List<DriverQualifier>();

            foreach (var d in driverGrids)
            {
                var driverQualifier = new DriverQualifier(
                    Guid.Parse(d.DriverId),
                    d.DriverName,
                    d.Position
                );
                driversQualifiers.Add(driverQualifier);
            }

            var race = new RaceControl(
                raceDto.Id,
                raceDto.Round,
                raceDto.YearSeason,
                new CircuitRace(
                    raceDto.Circuit.Id,
                    raceDto.Circuit.Name,
                    raceDto.Circuit.Country,
                    raceDto.Circuit.LapsNumber
                ),
                driversQualifiers
            );

            return race;
        }

        private async Task<RaceControl> AddGridFinalRaceInRaceControl(List<DriverGridFinalRaceResponseDTO> gridFinalRace, Guid raceId)
        {
            var race = await _raceControlRepository.GetRaceByIdAsync(raceId);

            foreach (var d in gridFinalRace)
            {
                var dto = new DriverRace(
                    Guid.Parse(d.DriverId),
                    d.DriverName,
                    Guid.Parse(d.TeamId),
                    d.TeamName,
                    d.Position,
                    d.Pontuation
                );
                race.GridRace.Add(dto);
            }

            return race;
        }

        public async Task<RaceControlResponseDTO> GetRaceByRaceIdAsync(Guid raceId)
        {
            try
            {
                var race = await _raceControlRepository.GetRaceByIdAsync(raceId);
                if (race is null)
                    throw new KeyNotFoundException("Race not found!");

                var response = TransformRaceControlModelObjectToResponseDTO(race);

                return response;
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<RaceControlResponseDTO>> GetRacesBySeasonYearAsync(int year)
        {
            try
            {
                var races = await _raceControlRepository.GetRacesBySeasonYearAsync(year);
                if (races is null || races.Count == 0)
                    throw new KeyNotFoundException("Races not found!");

                var responseList = new List<RaceControlResponseDTO>();
                foreach (var r in races)
                {
                    var response = TransformRaceControlModelObjectToResponseDTO(r);
                    responseList.Add(response);
                }

                return responseList;
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private RaceControlResponseDTO TransformRaceControlModelObjectToResponseDTO(RaceControl race)
        {
            var response = new RaceControlResponseDTO
            {
                RaceId = race.RaceId.ToString(),
                Round = race.Round,
                Season = race.Season,
                Circuit = new CircuitResponseDTO
                {
                    CircuitName = race.Circuit.CircuitName,
                    Country = race.Circuit.Country,
                    LapsNumber = race.Circuit.LapsNumber
                }
            };

            foreach (var d in race.GridQualifier)
            {
                response.DriversQualifier.Add(new DriverQualifierResponseDTO
                {
                    DriverName = d.DriverName,
                    Position = d.Position,
                });
            }

            if (race.GridRace.Count > 0)
            {
                foreach (var d in race.GridRace)
                {
                    response.DriversRace.Add(new DriverRaceResponseDTO
                    {
                        DriverName = d.DriverName,
                        TeamName = d.TeamName,
                        Position = d.Position,
                        Pontuation = d.Pontuation,
                    });
                }
            }

            return response;
        }
    }
}
