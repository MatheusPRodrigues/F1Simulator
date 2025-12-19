using F1Simulator.CompetitionService.Exceptions;
using F1Simulator.CompetitionService.Repositories;
using F1Simulator.CompetitionService.Repositories.Interfaces;
using F1Simulator.CompetitionService.Services.Interfaces;
using F1Simulator.Models.DTOs.CompetitionService.Response;
using F1Simulator.Models.DTOs.DriverDTO;
using F1Simulator.Models.DTOs.TeamManegementService.TeamDTO;
using F1Simulator.Models.Models;
using F1Simulator.Models.Enums.CompetitionService;
 using System.Security;
using F1Simulator.Models.DTOs.TeamManegementService.CarDTO;

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

        public CompetitionService(ILogger<ICompetitionService> logger, ICompetitionRepository competitionRepository, IHttpClientFactory httpClientFactory, ICircuitRepository circuitRepository)
        {
            _logger = logger;
            _competitionRepository = competitionRepository;
            _httpGetTeamsClient = httpClientFactory.CreateClient("GetTeamsClient");
            _httpGetDriversClient = httpClientFactory.CreateClient("GetDriversClient");
            _httpGetCountCars = httpClientFactory.CreateClient("GetCountCarsClient");
            _circuitRepository = circuitRepository;
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
                // verifica se já existe uma temporada ativa, se existir, lança uma exceção própria
                var activeSeason = await _competitionRepository.GetCompetionActiveAsync();
                if (activeSeason != null)
                {
                    throw new BusinessException("There is already a season in progress");
                }

                // Verifica se existem 10 ou 11 equipes cadastradas, se não houver, lança uma exceção própria
                var countTeams = await _httpGetTeamsClient.GetFromJsonAsync<int>("count");
                if (countTeams < 10 || countTeams > 11)
                {
                    throw new BusinessException("The season cannot start due to a lack of teams.");
                }

                // Verifica se cada equipe possui 2 pilotos cadastrados, se não possuir, lança uma exceção própria

                var countDrivers = await _httpGetDriversClient.GetFromJsonAsync<int>("count");
                if (countDrivers < countTeams * 2)
                {
                    throw new BusinessException("The season cannot start due to a lack of drivers.");
                }

                // verifica se cada piloto está associado a um carro, se não estiver, lança uma exceção própria

                var countCars = await _httpGetCountCars.GetFromJsonAsync<List<CarResponseDTO>>("");
                if ( countCars == null || countCars.Count < countDrivers)
                {
                    throw new BusinessException("The season cannot start due to a lack of cars.");
                }

                // verifica se existem 24 circuitos ativos, se não houver, lança uma exceção própria

                int countCircuits = await _circuitRepository.CircuitsActivatesAsync();
                if (countCircuits < 24)
                {
                    throw new BusinessException("The season cannot start due to a lack of circuits.");
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
                var driverStandings = responseDrivers.Select(d => new DriverStanding(d.DriverId, season.Id, 0, 0, d.FullName)).ToList();

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
                    throw new BusinessException("There is no active season to start the race.");
                }

                // buscar a corrida do round informado

                var race = await _competitionRepository.GetRaceCompleteByIdAndSeasonIdAsync(round, activeSeason.Id);
                if (race == null)
                {
                    return null;
                }

                // validar se a corrida já foi finalizada ou está em andamento
                if (race.Status == RaceStatus.Finished.ToString())
                {
                    throw new BusinessException("The race with this round has already finished.");
                }

                if (race.Status == RaceStatus.InProgress.ToString())
                {
                    throw new BusinessException("The race with this round is already underway.");
                }

                // Verifica se a corrida de round -1 foi finalizada, se não for lançar uma exceção própria

                if (round > 1)
                {
                    var previousRace = await _competitionRepository.GetRaceCompleteByIdAndSeasonIdAsync(round - 1, activeSeason.Id);
                    if (previousRace == null || previousRace.Status != RaceStatus.Finished.ToString())
                    {
                        throw new BusinessException("The previous race has not yet finished.");
                    }
                }

                // verificar se há alguma corrida em andamento na temporada ativa, se houver, lançar uma exceção própria

                var racesInProgress = await _competitionRepository.ExistRaceInProgressAsync(activeSeason.Id);
                if (racesInProgress)
                {
                    throw new BusinessException("There is already a race in progress in the current season.");
                }

                // se passar por tudo, iniciar a corrida mudando o status para "InProgress"

                await _competitionRepository.UpdateStatusRaceAsync(race.Id);

                // retornar as informações da corrida iniciada
                return await _competitionRepository.GetRaceByIdAsync(race.Id);
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
                if (activeSeason == null)
                {
                    throw new BusinessException("There is no active season.");
                }

                return await _competitionRepository.GetRaceWithCircuitAsync();

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
                if (activeSeason == null)
                {
                    throw new BusinessException("There is no active season.");
                }

                // buscar a corrida em andamento
                var raceInProgress = await _competitionRepository.GetRaceInProgressAsync();
                if (raceInProgress == null)
                {
                    throw new BusinessException("There is no active race.");
                }

                await _competitionRepository.UpdateRaceT1Async();
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
                if (activeSeason == null)
                {
                    throw new BusinessException("There is no active season.");
                }

                // buscar a corrida em andamento
                var raceInProgress = await _competitionRepository.GetRaceInProgressAsync();
                if (raceInProgress == null)
                {
                    throw new BusinessException("There is no active race.");
                }

                // verificar se o T1 já foi atualizado
                if (!raceInProgress.T1)
                {
                    throw new BusinessException("Free practice 1 must be completed before Free practice 2.");
                }

                await _competitionRepository.UpdateRaceT2Async();
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
                if (activeSeason == null)
                {
                    throw new BusinessException("There is no active season.");
                }

                // buscar a corrida em andamento
                var raceInProgress = await _competitionRepository.GetRaceInProgressAsync();
                if (raceInProgress == null)
                {
                    throw new BusinessException("There is no active race.");
                }

                // verificar se o T2 já foi atualizado
                if (!raceInProgress.T2)
                {
                    throw new BusinessException("Free practice 2 must be completed before Free practice 3.");
                }

                await _competitionRepository.UpdateRaceT3Async();
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
                if (activeSeason == null)
                {
                    throw new BusinessException("There is no active season.");
                }

                // buscar a corrida em andamento
                var raceInProgress = await _competitionRepository.GetRaceInProgressAsync();
                if (raceInProgress == null)
                {
                    throw new BusinessException("There is no active race.");
                }

                // verificar se o T3 já foi atualizado
                if (!raceInProgress.T3)
                {
                    throw new BusinessException("Free practice 3 must be completed before Qualifier.");
                }

                await _competitionRepository.UpdateRaceQualifierAsync();
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
                if (activeSeason == null)
                {
                    throw new BusinessException("There is no active season.");
                }

                // buscar a corrida em andamento
                var raceInProgress = await _competitionRepository.GetRaceInProgressAsync();
                if (raceInProgress == null)
                {
                    throw new BusinessException("There is no active race.");
                }

                // verificar se Qualifier já foi atualizado
                if (!raceInProgress.Qualifier)
                {
                    throw new BusinessException("Qualifier must be completed before Race.");
                }

                await _competitionRepository.UpdateRaceRaceAsync();
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
                if (activeSeason == null)
                {
                    throw new BusinessException("There is no active season.");
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
                if (activeSeason == null)
                {
                    throw new BusinessException("There is no active season.");
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
                if (activeSeason == null)
                {
                    throw new BusinessException("There is no active season.");
                }
                return await _competitionRepository.GetRacesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetRacesAsync in CompetitionService");
                throw;
            }
        }



    }


}
