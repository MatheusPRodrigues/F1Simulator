using F1Simulator.Models.DTOs.RaceControlService;
using F1Simulator.RaceControlService.Messaging;
using F1Simulator.RaceControlService.Repositories.Interfaces;
using F1Simulator.RaceControlService.Services.Interfaces;

namespace F1Simulator.RaceControlService.Services
{
    public class RaceControlService : IRaceControlService
    {
        private readonly IPublishService _messageService;
        private readonly IRaceControlRepository _raceControlRepository;
        private readonly ILogger<RaceControlService> _logger;

        public RaceControlService(
            IPublishService messageService,
            IRaceControlRepository raceControlRepository,
            ILogger<RaceControlService> logger
        )
        {
            _messageService = messageService; 
            _raceControlRepository = raceControlRepository;
            _logger = logger;
        }

        public Task ExecuteQualifierSectionAsync(string raceId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DriverGridFinalRaceResponseDTO>> ExecuteRaceSectionAsync(string raceId)
        {
            try
            {
                const string PUBLISHQUEUE = "RaceFinishedEvent";
                int[] pontuationArray = { 25, 18, 15, 12, 10, 8, 6, 4, 2, 1 };
                var driversToRace = new List<DriverToRaceDTO>
                {
                    new()
                    {
                        DriverId = Guid.NewGuid().ToString(),
                        DriverName = "Max Verstappen",
                        Handicap = 0.95,
                        DriverExp = 95,
                        TeamId = Guid.NewGuid().ToString(),
                        TeamName = "Red Bull Racing",
                        EnginneringAId = Guid.NewGuid().ToString(),
                        EnginneringPId = Guid.NewGuid().ToString(),
                        CarId = Guid.NewGuid().ToString(),
                        Ca = 0.92,
                        Cp = 0.94
                    },
                    new()
                    {
                        DriverId = Guid.NewGuid().ToString(),
                        DriverName = "Lewis Hamilton",
                        Handicap = 0.97,
                        DriverExp = 98,
                        TeamId = Guid.NewGuid().ToString(),
                        TeamName = "Mercedes",
                        EnginneringAId = Guid.NewGuid().ToString(),
                        EnginneringPId = Guid.NewGuid().ToString(),
                        CarId = Guid.NewGuid().ToString(),
                        Ca = 0.90,
                        Cp = 0.96
                    },
                    new()
                    {
                        DriverId = Guid.NewGuid().ToString(),
                        DriverName = "Charles Leclerc",
                        Handicap = 0.94,
                        DriverExp = 88,
                        TeamId = Guid.NewGuid().ToString(),
                        TeamName = "Ferrari",
                        EnginneringAId = Guid.NewGuid().ToString(),
                        EnginneringPId = Guid.NewGuid().ToString(),
                        CarId = Guid.NewGuid().ToString(),
                        Ca = 0.91,
                        Cp = 0.93
                    },
                    new()
                    {
                        DriverId = Guid.NewGuid().ToString(),
                        DriverName = "Lando Norris",
                        Handicap = 0.93,
                        DriverExp = 85,
                        TeamId = Guid.NewGuid().ToString(),
                        TeamName = "McLaren",
                        EnginneringAId = Guid.NewGuid().ToString(),
                        EnginneringPId = Guid.NewGuid().ToString(),
                        CarId = Guid.NewGuid().ToString(),
                        Ca = 0.89,
                        Cp = 0.91
                    },
                    new()
                    {
                        DriverId = Guid.NewGuid().ToString(),
                        DriverName = "Fernando Alonso",
                        Handicap = 0.96,
                        DriverExp = 99,
                        TeamId = Guid.NewGuid().ToString(),
                        TeamName = "Aston Martin",
                        EnginneringAId = Guid.NewGuid().ToString(),
                        EnginneringPId = Guid.NewGuid().ToString(),
                        CarId = Guid.NewGuid().ToString(),
                        Ca = 0.88,
                        Cp = 0.95
                    },

                    // 15 pilotos genéricos
                    new() { DriverId = Guid.NewGuid().ToString(), DriverName = "Driver 06", Handicap = 0.90, DriverExp = 80, TeamId = Guid.NewGuid().ToString(), TeamName = "Team A", EnginneringAId = Guid.NewGuid().ToString(), EnginneringPId = Guid.NewGuid().ToString(), CarId = Guid.NewGuid().ToString(), Ca = 0.87, Cp = 0.88 },
                    new() { DriverId = Guid.NewGuid().ToString(), DriverName = "Driver 07", Handicap = 0.89, DriverExp = 78, TeamId = Guid.NewGuid().ToString(), TeamName = "Team B", EnginneringAId = Guid.NewGuid().ToString(), EnginneringPId = Guid.NewGuid().ToString(), CarId = Guid.NewGuid().ToString(), Ca = 0.86, Cp = 0.87 },
                    new() { DriverId = Guid.NewGuid().ToString(), DriverName = "Driver 08", Handicap = 0.88, DriverExp = 76, TeamId = Guid.NewGuid().ToString(), TeamName = "Team C", EnginneringAId = Guid.NewGuid().ToString(), EnginneringPId = Guid.NewGuid().ToString(), CarId = Guid.NewGuid().ToString(), Ca = 0.85, Cp = 0.86 },
                    new() { DriverId = Guid.NewGuid().ToString(), DriverName = "Driver 09", Handicap = 0.87, DriverExp = 75, TeamId = Guid.NewGuid().ToString(), TeamName = "Team D", EnginneringAId = Guid.NewGuid().ToString(), EnginneringPId = Guid.NewGuid().ToString(), CarId = Guid.NewGuid().ToString(), Ca = 0.84, Cp = 0.85 },
                    new() { DriverId = Guid.NewGuid().ToString(), DriverName = "Driver 10", Handicap = 0.86, DriverExp = 74, TeamId = Guid.NewGuid().ToString(), TeamName = "Team E", EnginneringAId = Guid.NewGuid().ToString(), EnginneringPId = Guid.NewGuid().ToString(), CarId = Guid.NewGuid().ToString(), Ca = 0.83, Cp = 0.84 },

                    new() { DriverId = Guid.NewGuid().ToString(), DriverName = "Driver 11", Handicap = 0.85, DriverExp = 73, TeamId = Guid.NewGuid().ToString(), TeamName = "Team F", EnginneringAId = Guid.NewGuid().ToString(), EnginneringPId = Guid.NewGuid().ToString(), CarId = Guid.NewGuid().ToString(), Ca = 0.82, Cp = 0.83 },
                    new() { DriverId = Guid.NewGuid().ToString(), DriverName = "Driver 12", Handicap = 0.84, DriverExp = 72, TeamId = Guid.NewGuid().ToString(), TeamName = "Team G", EnginneringAId = Guid.NewGuid().ToString(), EnginneringPId = Guid.NewGuid().ToString(), CarId = Guid.NewGuid().ToString(), Ca = 0.81, Cp = 0.82 },
                    new() { DriverId = Guid.NewGuid().ToString(), DriverName = "Driver 13", Handicap = 0.83, DriverExp = 71, TeamId = Guid.NewGuid().ToString(), TeamName = "Team H", EnginneringAId = Guid.NewGuid().ToString(), EnginneringPId = Guid.NewGuid().ToString(), CarId = Guid.NewGuid().ToString(), Ca = 0.80, Cp = 0.81 },
                    new() { DriverId = Guid.NewGuid().ToString(), DriverName = "Driver 14", Handicap = 0.82, DriverExp = 70, TeamId = Guid.NewGuid().ToString(), TeamName = "Team I", EnginneringAId = Guid.NewGuid().ToString(), EnginneringPId = Guid.NewGuid().ToString(), CarId = Guid.NewGuid().ToString(), Ca = 0.79, Cp = 0.80 },
                    new() { DriverId = Guid.NewGuid().ToString(), DriverName = "Driver 15", Handicap = 0.81, DriverExp = 69, TeamId = Guid.NewGuid().ToString(), TeamName = "Team J", EnginneringAId = Guid.NewGuid().ToString(), EnginneringPId = Guid.NewGuid().ToString(), CarId = Guid.NewGuid().ToString(), Ca = 0.78, Cp = 0.79 },

                    new() { DriverId = Guid.NewGuid().ToString(), DriverName = "Driver 16", Handicap = 0.80, DriverExp = 68, TeamId = Guid.NewGuid().ToString(), TeamName = "Team K", EnginneringAId = Guid.NewGuid().ToString(), EnginneringPId = Guid.NewGuid().ToString(), CarId = Guid.NewGuid().ToString(), Ca = 0.77, Cp = 0.78 },
                    new() { DriverId = Guid.NewGuid().ToString(), DriverName = "Driver 17", Handicap = 0.79, DriverExp = 67, TeamId = Guid.NewGuid().ToString(), TeamName = "Team L", EnginneringAId = Guid.NewGuid().ToString(), EnginneringPId = Guid.NewGuid().ToString(), CarId = Guid.NewGuid().ToString(), Ca = 0.76, Cp = 0.77 },
                    new() { DriverId = Guid.NewGuid().ToString(), DriverName = "Driver 18", Handicap = 0.78, DriverExp = 66, TeamId = Guid.NewGuid().ToString(), TeamName = "Team M", EnginneringAId = Guid.NewGuid().ToString(), EnginneringPId = Guid.NewGuid().ToString(), CarId = Guid.NewGuid().ToString(), Ca = 0.75, Cp = 0.76 },
                    new() { DriverId = Guid.NewGuid().ToString(), DriverName = "Driver 19", Handicap = 0.77, DriverExp = 65, TeamId = Guid.NewGuid().ToString(), TeamName = "Team N", EnginneringAId = Guid.NewGuid().ToString(), EnginneringPId = Guid.NewGuid().ToString(), CarId = Guid.NewGuid().ToString(), Ca = 0.74, Cp = 0.75 },
                    new() { DriverId = Guid.NewGuid().ToString(), DriverName = "Driver 20", Handicap = 0.76, DriverExp = 64, TeamId = Guid.NewGuid().ToString(), TeamName = "Team O", EnginneringAId = Guid.NewGuid().ToString(), EnginneringPId = Guid.NewGuid().ToString(), CarId = Guid.NewGuid().ToString(), Ca = 0.73, Cp = 0.74 }
                };

                var luck = Random.Shared.Next(1, 10);

                var driverProcessToGrid = new List<DriverGridFinalRaceResponseDTO>();

                foreach (var d in driversToRace)
                {
                    // recebe novos valores de Ca e Cp
                    var newCa = d.Ca + 1;
                    var newCp = d.Cp + 1;

                    var dto = new DriverGridFinalRaceResponseDTO
                    {
                        DriverId = d.DriverId,
                        DriverName = d.DriverName,
                        Handicap = d.Handicap - (d.DriverExp * 0.5),
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
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while execute race section: {ex}");
                throw;
            }
        }

        public Task ExecuteTlOneSectionAsync(string raceId)
        {
            throw new NotImplementedException();
        }

        public Task ExecuteTlThreeSectionAsync(string raceId)
        {
            throw new NotImplementedException();
        }

        public Task ExecuteTlTwoSectionAsync(string raceId)
        {
            throw new NotImplementedException();
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
