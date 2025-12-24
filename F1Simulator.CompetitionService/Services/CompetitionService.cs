using F1Simulator.CompetitionService.Repositories.Interfaces;
using F1Simulator.CompetitionService.Services.Interfaces;
using F1Simulator.Models.DTOs.CompetitionService.Response;
using F1Simulator.Models.DTOs.DriverDTO;
using F1Simulator.Models.DTOs.RaceControlService;
using F1Simulator.Models.DTOs.TeamManegementService.CarDTO;
using F1Simulator.Models.DTOs.TeamManegementService.TeamDTO;
using F1Simulator.Models.Enums.CompetitionService;
using F1Simulator.Models.Models;
using F1Simulator.Models.Models.CompetitionService;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;


namespace F1Simulator.CompetitionService.Services
{
    public class CompetitionService : ICompetitionService
    {
        private readonly ILogger<ICompetitionService> _logger;
        private readonly ICompetitionRepository _competitionRepository;
        private readonly ICircuitRepository _circuitRepository;
        private readonly HttpClient _httpGetTeamsClient;
        private readonly HttpClient _httpGetDriversClient;
        private readonly HttpClient _httpGetCountCars;
        private readonly IConnectionFactory _connectionFactory;
        private readonly HttpClient _httpGetCountEngineers;

        public CompetitionService(ILogger<ICompetitionService> logger,
            ICompetitionRepository competitionRepository,
            IHttpClientFactory httpClientFactory,
            ICircuitRepository circuitRepository,
            IConnectionFactory _connection)
        {
            _logger = logger;
            _competitionRepository = competitionRepository;
            _httpGetTeamsClient = httpClientFactory.CreateClient("GetTeamsClient");
            _httpGetDriversClient = httpClientFactory.CreateClient("GetDriversClient");
            _httpGetCountCars = httpClientFactory.CreateClient("GetCountCarsClient");
            _httpGetCountEngineers = httpClientFactory.CreateClient("GetCountEngineersClient");
            _circuitRepository = circuitRepository;
            _connectionFactory = _connection;
        }

        public async Task<SeasonResponseDTO?> GetCompetitionActiveAsync()
        {
            try
            {
                return await _competitionRepository.GetCompetionActiveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetCompetitionActiveAsync in CompetitionService");
                throw;
            }
        }


        public async Task<SeasonResponseDTO?> StartSeasonAsync()
        {
            try
            {
                // verifica se já existe uma temporada ativa
                var activeSeason = await _competitionRepository.GetCompetionActiveAsync();
                if (activeSeason != null)
                {
                    throw new InvalidOperationException("There is already a season in progress");
                }

                // Verifica se existem 10 ou 11 equipes cadastradas, se não houver lança uma exceção 
                var countTeams = await _httpGetTeamsClient.GetFromJsonAsync<int>("count");
                if (countTeams < 10 || countTeams > 11)
                {
                    throw new InvalidOperationException("The season cannot start due to a lack of teams.");
                }

                // Verifica se existem 40 ou 44 engenheiros cadastradas, se não houver lança uma exceção 
                var countEngineers = await _httpGetCountEngineers.GetFromJsonAsync<int>("count");
                if (countEngineers != 40 && countEngineers != 44)
                {
                    throw new InvalidOperationException("The season cannot start due to a lack of engineers.");
                }


                // Verifica se cada equipe possui 2 pilotos cadastrados, se não possuir lança uma exceção 

                var countDrivers = await _httpGetDriversClient.GetFromJsonAsync<int>("count");
                if (countDrivers < countTeams * 2)
                {
                    throw new InvalidOperationException("The season cannot start due to a lack of drivers.");
                }

                // verifica se cada piloto está associado a um carro, se não estiver lança uma exceção 

                var countCars = await _httpGetCountCars.GetFromJsonAsync<List<CarResponseDTO>>("");
                if (countCars == null || countCars.Count < countDrivers)
                {
                    throw new InvalidOperationException("The season cannot start due to a lack of cars.");
                }

                // verifica se existem 24 circuitos ativos, se não houver, lança uma exceção 

                int countCircuits = await _circuitRepository.CircuitsActivatesAsync();
                if (countCircuits < 24)
                {
                    throw new InvalidOperationException("The season cannot start due to a lack of circuits.");
                }

                // se todas as validações forem passadas, importar as equipes e pilotos e circuits

                var responseTeams = await _httpGetTeamsClient.GetFromJsonAsync<List<TeamResponseDTO>>("");
                var responseDrivers = await _httpGetDriversClient.GetFromJsonAsync<List<DriverResponseDTO>>("");
                var responseCircuits = await _circuitRepository.GetAllCircuitsActiveAsync();

                // criar a nova temporada
                Season season;
                var year = await _competitionRepository.GetMaxYearSeasonAsync();
                if (year == null)
                {
                    season = new Season(2025);
                }
                else
                {
                    season = new Season(year.Value + 1);
                }

                // construir os standings de equipes e pilotos com 0 pontos, e gerar o calendário com os circuitos ativos

                var teamStandings = responseTeams.Select(t => new TeamStanding(season.Id, t.TeamId, 0, t.Name)).ToList();
                var driverStandings = responseDrivers.Select(d => new DriverStanding(d.DriverId, season.Id, Guid.Parse(d.TeamId), 0, 0, d.FullName)).ToList();

                var races = new List<Race>();
                int round = 1;
                foreach (var circuit in responseCircuits)
                {
                    races.Add(new Race(season.Id, circuit.Id, round));
                    round++;
                }
                await _competitionRepository.StartSeasonAsync(season, teamStandings, driverStandings, races);

                return new SeasonResponseDTO
                {
                    Id = season.Id,
                    Year = season.Year,
                    IsActive = season.IsActive
                };
            }catch(InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in StartSeasonAsync in CompetitionService");
                throw;
            }
        }

        public async Task<RaceResponseDTO?> StartRaceAsync(int round)
        {
            try
            {
                // buscar a temporada ativa

                var activeSeason = await _competitionRepository.GetCompetionActiveAsync();
                if (activeSeason == null)
                {
                    throw new InvalidOperationException("There is no active season to start the race.");
                }

                // validar round informado

                if(round < 0 || round > 24)
                {
                    throw new ArgumentException("The reported Round must be between 1 and 24.");
                }

                // buscar a corrida do round informado

                var race = await _competitionRepository.GetRaceCompleteByIdAndSeasonIdAsync(round, activeSeason.Id);
                if (race is null)
                {
                    throw new KeyNotFoundException("No races were found matching the reported round number."); 
                }

                // validar se a corrida já foi finalizada ou está em andamento
                if (race.Status == RaceStatus.Finished.ToString())
                {
                    throw new InvalidOperationException("The race with this round has already finished.");
                }

                if (race.Status == RaceStatus.InProgress.ToString())
                {
                    throw new InvalidOperationException("The race with this round is already underway.");
                }

                // Verifica se a corrida de round -1 foi finalizada

                if (round > 1)
                {
                    var previousRace = await _competitionRepository.GetRaceCompleteByIdAndSeasonIdAsync(round - 1, activeSeason.Id);
                    if (previousRace == null || previousRace.Status != RaceStatus.Finished.ToString())
                    {
                        throw new InvalidOperationException("The previous race has not yet finished.");
                    }
                }

                // verificar se há alguma corrida em andamento na temporada ativa, se houver, lançar uma exceção

                var racesInProgress = await _competitionRepository.ExistRaceInProgressAsync(activeSeason.Id);
                if (racesInProgress)
                {
                    throw new InvalidOperationException("There is already a race in progress in the current season.");
                }

                // se passar por tudo, iniciar a corrida mudando o status para "InProgress"

                await _competitionRepository.UpdateStatusRaceAsync(race.Id);

                // retornar as informações da corrida iniciada
                return await _competitionRepository.GetRaceByIdAsync(race.Id);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in StartRaceAsync in CompetitionService");
                throw;
            }
        }

        public async Task<RaceWithCircuitResponseDTO?> GetRaceWithCircuitAsync()
        {
            try
            {
                // buscar a temporada ativa
                var activeSeason = await _competitionRepository.GetCompetionActiveAsync();
                if (activeSeason is null)
                {
                    throw new InvalidOperationException("There is no active season.");
                }

                var response = await _competitionRepository.GetRaceWithCircuitAsync();

                return response;
            }
            catch(InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetRaceWithCircuitAsync in CompetitionService");
                throw;
            }
        }

        public async Task UpdateRaceT1Async()
        {
            try
            {
                // buscar a temporada ativa
                var activeSeason = await _competitionRepository.GetCompetionActiveAsync();
                if (activeSeason is null)
                {
                    throw new InvalidOperationException("There is no active season.");
                }

                // buscar a corrida em andamento
                var raceInProgress = await _competitionRepository.GetRaceInProgressAsync();
                if (raceInProgress is null)
                {
                    throw new InvalidOperationException("There is no active race.");
                }

                // vericar se t1 já foi atualizado
                if (raceInProgress.T1)
                {
                    throw new InvalidOperationException("Free practice 1 has already been completed.");
                }

                await _competitionRepository.UpdateRaceT1Async();
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateRaceT1Async in CompetitionService");
                throw;

            }
        }

        public async Task UpdateRaceT2Async()
        {
            try
            {
                // buscar a temporada ativa
                var activeSeason = await _competitionRepository.GetCompetionActiveAsync();
                if (activeSeason is null)
                {
                    throw new InvalidOperationException("There is no active season.");
                }

                // buscar a corrida em andamento
                var raceInProgress = await _competitionRepository.GetRaceInProgressAsync();
                if (raceInProgress is null)
                {
                    throw new InvalidOperationException("There is no active race.");
                }

                // verificar se o T1 já foi atualizado
                if (!raceInProgress.T1)
                {
                    throw new InvalidOperationException("Free practice 1 must be completed before Free practice 2.");
                }

                // vericar se t2 já foi atualizado
                if (raceInProgress.T2)
                {
                    throw new InvalidOperationException("Free practice 2 has already been completed.");
                }

                await _competitionRepository.UpdateRaceT2Async();
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateRaceT2Async in CompetitionService");
                throw;

            }
        }


        public async Task UpdateRaceT3Async()
        {
            try
            {
                // buscar a temporada ativa
                var activeSeason = await _competitionRepository.GetCompetionActiveAsync();
                if (activeSeason is null)
                {
                    throw new InvalidOperationException("There is no active season.");
                }

                // buscar a corrida em andamento
                var raceInProgress = await _competitionRepository.GetRaceInProgressAsync();
                if (raceInProgress is null)
                {
                    throw new InvalidOperationException("There is no active race.");
                }

                // verificar se o T2 já foi atualizado
                if (!raceInProgress.T2)
                {
                    throw new InvalidOperationException("Free practice 2 must be completed before Free practice 3.");
                }

                // vericar se t3 já foi atualizado
                if (raceInProgress.T3)
                {
                    throw new InvalidOperationException("Free practice 3 has already been completed.");
                }

                await _competitionRepository.UpdateRaceT3Async();
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateRaceT3Async in CompetitionService");
                throw;

            }
        }

        public async Task UpdateRaceQualifierAsync()
        {
            try
            {
                // buscar a temporada ativa
                var activeSeason = await _competitionRepository.GetCompetionActiveAsync();
                if (activeSeason is null)
                {
                    throw new InvalidOperationException("There is no active season.");
                }

                // buscar a corrida em andamento
                var raceInProgress = await _competitionRepository.GetRaceInProgressAsync();
                if (raceInProgress is null)
                {
                    throw new InvalidOperationException("There is no active race.");
                }

                // verificar se o T3 já foi atualizado
                if (!raceInProgress.T3)
                {
                    throw new InvalidOperationException("Free practice 3 must be completed before Qualifier.");
                }

                // vericar se Qualifier já foi atualizado
                if (raceInProgress.Qualifier)
                {
                    throw new InvalidOperationException("Qualifier has already been completed.");
                }

                await _competitionRepository.UpdateRaceQualifierAsync();
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateRaceQualifierAsync in CompetitionService");
                throw;

            }
        }

        public async Task UpdateRaceRaceAsync()
        {
            try
            {
                // buscar a temporada ativa
                var activeSeason = await _competitionRepository.GetCompetionActiveAsync();
                if (activeSeason is null)
                {
                    throw new InvalidOperationException("There is no active season.");
                }

                // buscar a corrida em andamento
                var raceInProgress = await _competitionRepository.GetRaceInProgressAsync();
                if (raceInProgress is null)
                {
                    throw new InvalidOperationException("There is no active race.");
                }

                // verificar se Qualifier já foi atualizado
                if (!raceInProgress.Qualifier)
                {
                    throw new InvalidOperationException("Qualifier must be completed before Race.");
                }

                // vericar se Race já foi atualizado
                if (raceInProgress.RaceFinal)
                {
                    throw new InvalidOperationException("Race has already been completed");
                }

                await _competitionRepository.UpdateRaceRaceAsync();
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateRaceRaceAsync in CompetitionService");
                throw;

            }
        }

        public async Task<List<DriverStandingResponseWhitPositionDTO>> GetDriverStandingAsync()
        {
            try
            {
                // verificar se existe uma temporada ativa

                var activeSeason = await _competitionRepository.GetCompetionActiveAsync();
                if (activeSeason is null)
                {
                    throw new InvalidOperationException("There is no active season.");
                }


                var driverStanding = await _competitionRepository.GetDriverStandingAsync();
                List<DriverStandingResponseWhitPositionDTO> driverStandingResponse = new List<DriverStandingResponseWhitPositionDTO>();
                int contador = 1;
                foreach (var ds in driverStanding)
                {
                    driverStandingResponse.Add(new DriverStandingResponseWhitPositionDTO
                    {
                        Position = contador,
                        DriverName = ds.DriverName,
                        Points = ds.Points
                    });
                    contador++;
                }

                return driverStandingResponse;
            }
            catch(InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetDriverStanding in CompetitionService");
                throw;
            }
        }

        public async Task<List<TeamStandingResponseWhitPositionDTO>> GetTeamStandingAsync()
        {
            try
            {
                // verificar se existe uma temporada ativa

                var activeSeason = await _competitionRepository.GetCompetionActiveAsync();
                if (activeSeason is null)
                {
                    throw new InvalidOperationException("There is no active season.");
                }


                var teamStanding = await _competitionRepository.GetTeamStandingAsync();
                List<TeamStandingResponseWhitPositionDTO> teamStandingResponse = new List<TeamStandingResponseWhitPositionDTO>();
                int contador = 1;
                foreach (var ts in teamStanding)
                {
                    teamStandingResponse.Add(new TeamStandingResponseWhitPositionDTO
                    {
                        Position = contador,
                        TeamName = ts.TeamName,
                        Points = ts.Points
                    });
                    contador++;
                }

                return teamStandingResponse;
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTeamStanding in CompetitionService");
                throw;
            }
        }

        public async Task<List<RaceResponseDTO>> GetRacesAsync()
        {
            try
            {
                // verificar se existe uma temporada ativa
                var activeSeason = await _competitionRepository.GetCompetionActiveAsync();
                if (activeSeason is null)
                {
                    throw new InvalidOperationException("There is no active season.");
                }
                return await _competitionRepository.GetRacesAsync();
            }
            catch(InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetRacesAsync in CompetitionService");
                throw;
            }
        }

        private async Task<List<DriverToPublishDTO>> ConsumeRaceResultsAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            using (var channel = await connection.CreateChannelAsync())
            {                
                await channel.QueueDeclareAsync(queue: "RaceFinishedEvent",
                                               durable: false,
                                               exclusive: false,
                                               autoDelete: false,
                                               arguments: null);

                var consumer = new AsyncEventingBasicConsumer(channel);
                uint numeroMensagens = await channel.MessageCountAsync("RaceFinishedEvent");
                if (numeroMensagens == 0)
                {
                    throw new ArgumentException("Race results queue is empty");
                }

                var results = new List<DriverToPublishDTO>();
                var semaphore = new SemaphoreSlim(0, (int)numeroMensagens);

                consumer = new AsyncEventingBasicConsumer(channel);
                consumer.ReceivedAsync += async (model, ea) =>
                {
                    try
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);

                        var dto = JsonSerializer.Deserialize<List<DriverToPublishDTO>>(message);

                        foreach (var d in dto)
                        {
                            results.Add(d);
                        }

                        await channel.BasicAckAsync(ea.DeliveryTag, false);
                    }
                    catch (Exception ex)
                    {
                        await channel.BasicNackAsync(ea.DeliveryTag, false, true);
                        throw;
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                };
                await channel.BasicConsumeAsync(
                queue: "RaceFinishedEvent",
                autoAck: false,
                consumer: consumer);

                for (int i = 0; i < numeroMensagens; i++)
                {
                    await semaphore.WaitAsync();
                }

                return results;
            }

        }
        public async Task EndRaceAsync()
        {
            try
            {
                // buscar a temporada ativa
                var activeSeason = await _competitionRepository.GetCompetionActiveAsync();
                if (activeSeason is null)
                {
                    throw new InvalidOperationException("There is no active season.");
                }
                // buscar a corrida em andamento
                var raceInProgress = await _competitionRepository.GetRaceInProgressAsync();
                if (raceInProgress is null)
                {
                    throw new InvalidOperationException("There is no active race to end.");
                }

                // Consume da fila para processar os resultados da corrida

                var raceResults = await ConsumeRaceResultsAsync();

                // Buscar as listas de classificação do banco

                var drivers = await _competitionRepository.GetDriverStandingAsync();
                var teams = await _competitionRepository.GetTeamStandingAsync();
                List<DriverStandingResponseDTO> driversUpdate = new List<DriverStandingResponseDTO>();
                List<TeamStandingResponseDTO> teamsUpdate = new List<TeamStandingResponseDTO>();

                // atualizar listas de classificação

                if (drivers is not null)
                {
                    driversUpdate = UpdateDrivers(drivers, raceResults);
                }
                if (teams is not null)
                {
                    teamsUpdate = UpdateTeams(teams, drivers);
                }

                // enviar as listas de classificação para o banco e sobrescrever               

                await _competitionRepository.EndRaceAsync(driversUpdate, teamsUpdate, activeSeason, raceInProgress);
            }
            catch(InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in EndRaceAsync in CompetitionService");
                throw;
            }

        }

        private List<DriverStandingResponseDTO> UpdateDrivers(List<DriverStandingResponseDTO> currentDrivers, List<DriverToPublishDTO> raceResults)
        {
            foreach (var raceResult in raceResults)
            {
                var driver = currentDrivers
                    .First(d => d.DriverId.ToString() == raceResult.DriverId);

                driver.Points += raceResult.Pontuation;
            }

            return currentDrivers;
        }

        private List<TeamStandingResponseDTO> UpdateTeams(List<TeamStandingResponseDTO> currentTeams, List<DriverStandingResponseDTO> updatedDrivers)
        {
            foreach (var team in currentTeams)
            {
                team.Points = updatedDrivers
                    .Where(d => d.TeamId == team.TeamId)
                    .Sum(d => d.Points);
            }

            return currentTeams;
        }


        public async Task<StandingsResponseDTO> EndSeasonAsync()
        {
            try
            {
                // verificar se tem um season ativa 
                var activeSeason = await _competitionRepository.GetCompetionActiveAsync();
                if (activeSeason is null)
                {
                    throw new InvalidOperationException("There is no active season.");
                }

                // buscar a corrida round 24
                var race24 = await _competitionRepository.GetLastRaceRoundAsync();
                if (race24 is null)
                {
                    throw new KeyNotFoundException("error in searching for sequence race 24.");
                }

                if(race24.Status != "Finished")
                {
                    throw new InvalidOperationException("There are still races to be held.");
                }

                var driverStanding = await GetDriverStandingAsync();
                var teamStanding = await GetTeamStandingAsync();
                await _competitionRepository.EndSeasonAsync(activeSeason.Id);

                var standingsResponse = new StandingsResponseDTO
                {
                    DriverStandings = driverStanding,
                    TeamStandings = teamStanding
                };

                return standingsResponse;

            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in FinalStnadingsAsync in CompetitionService");
                throw;
            }

        }

        public async Task<List<SeasonResponseDTO>> GetAllSeasonsAsync()
        {
            try
            {
                return await _competitionRepository.GetAllSeasonsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetSeasonsAsync in CompetitionService");
                throw;
            }
        }
    }

}
