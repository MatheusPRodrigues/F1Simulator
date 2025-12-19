using Dapper;
using F1Simulator.CompetitionService.Data;
using F1Simulator.CompetitionService.Repositories.Interfaces;
using F1Simulator.Models.DTOs.CompetitionService.Response;
using F1Simulator.Models.Models;
using Microsoft.Data.SqlClient;

namespace F1Simulator.CompetitionService.Repositories
{
    public class CompetitionRepository : ICompetitionRepository
    {

        private readonly ILogger<CompetitionRepository> _logger;
        private readonly SqlConnection _connection;
        public CompetitionRepository(ILogger<CompetitionRepository> logger, CompetitionServiceConnection connection)
        {
            _logger = logger;
            _connection = connection.GetConnection();
        }

        public async Task<SeasonResponseDTO?> GetCompetionActive()
        {
            try
            {
                var query = "SELECT Id, Year, IsActive FROM Season WHERE IsActive = 1";

                return await _connection.QueryFirstOrDefaultAsync<SeasonResponseDTO>(query);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetCompetionActive in CompetitionRepository");
                throw;
            }
        }

        public async Task<int?> GetMaxYearSeason()
        {
            var query = "SELECT MAX ([Year]) FROM Season;";

            int? year = await _connection.QueryFirstOrDefaultAsync<int?>(query);
            return year;
        }

        public async Task<int> CreateSeason(int year)
        {
            var query = "INSERT INTO Season ([Year], IsActive) VALUES (@Year, 1); SELECT CAST(SCOPE_IDENTITY() as int);";
            var seasonId = await _connection.QuerySingleAsync<int>(query, new { Year = year });
            return seasonId;
        }

        public async Task StartSeasonAsync(Season season, List<TeamStanding> teams, List<DriverStanding> drivers, List<Race> races)
        {
            using var transaction = _connection.BeginTransaction();
            try
            {
                // Insert Season
                var insertSeasonQuery = "INSERT INTO Season (Id, [Year], IsActive) VALUES (@Id, @Year, @IsActive);";
                await _connection.ExecuteAsync(insertSeasonQuery, new { Id = season.Id, Year = season.Year, IsActive = season.IsActive }, transaction);

                // Insert Team Standings
                var insertTeamStandingsQuery = "INSERT INTO TeamStanding (Id, SeasonId, TeamName, TeamId, Points) VALUES (@Id, @SeasonId, @TeamName @TeamId, @Points);";
                foreach (var team in teams)
                {
                    await _connection.ExecuteAsync(insertTeamStandingsQuery, new { Id = team.Id, SeasonId = team.SeasonId, TeamName = team.TeamName, TeamId = team.TeamId, Points = team.Points }, transaction);
                }

                // Insert Driver Standings

                var insertDriverStandingsQuery = "INSERT INTO DriverStanding (Id, SeasonId, DriverName, DriverId, Position, Points) VALUES (@Id, @SeasonId, @DriverName, @DriverId, @Points);";
                foreach (var driver in drivers)
                {
                    await _connection.ExecuteAsync(insertDriverStandingsQuery, new
                    {
                        Id = driver.Id,
                        SeasonId = driver.SeasonId,
                        DriverName = driver.DriverName,
                        DriverId = driver.DriverId,
                        Points = driver.Points
                    }, transaction);
                }

                // Insert Races
                var insertRacesQuery = "INSERT INTO Races (Id, [Round], [Status], SeasonId, CircuitId, T1, T2, T3, Qualifier, Race) " +
                                       "VALUES (@Id, @Round, @Status, @SeasonId, @CircuitId, @T1, @T2, @T3, @Qualifier, @Race);";
                foreach (var race in races)
                {
                    await _connection.ExecuteAsync(insertRacesQuery, new
                    {
                        Id = race.Id,
                        Round = race.Round,
                        Status = race.Status,
                        SeasonId = race.SeasonId,
                        CircuitId = race.CircuitId,
                        T1 = race.T1,
                        T2 = race.T2,
                        T3 = race.T3,
                        Qualifier = race.Qualifier,
                        Race = race.RaceFinal
                    }, transaction);
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error in StartSeasonAsync in CompetitionRepository");
                throw;
            }
        }

        public async Task<RaceCompleteResponseDTO?> GetRaceCompleteByIdAndSeasonIdAsync(int round, Guid seasonID)
        {
            try
            {
                var selectQuery = @"SELECT Id, [Round], [Status], SeasonId, CircuitId, T1, T2, T3, Qualifier
                                   FROM Races
                                   WHERE SeasonId = @SeasonId
                                   AND [Round] = @Round";

                return await _connection.QueryFirstOrDefaultAsync<RaceCompleteResponseDTO>(selectQuery, new { SeasonId = seasonID, Round = round });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetRaceCompleteByIdAndSeasonIdAsync in CompetitionRepository");
                throw;
            }
        }

        public async Task<bool> ExistRaceInProgressAsync(Guid seasonID)
        {
            try
            {
                var selectQuery = @"SELECT COUNT(1)
                                    FROM Race
                                    WHERE SeasonId = @SeasonId
                                    AND Status = 'InProgress'";
                var count = await _connection.QuerySingleAsync<int>(selectQuery, new { SeasonId = seasonID });
                return count > 0;

            } catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ExistRaceInProgress in CompetitionRepository");
                throw;
            }
        }

        public async Task<RaceCompleteResponseDTO?> GetRaceInProgress()
        {
            try
            {
                var selectQuery = @"SELECT Id, [Round], [Status], SeasonId, CircuitId, T1, T2, T3, Qualifier
                                   FROM Races
                                   WHERE [Status] = 'InProgress'";
                return await _connection.QueryFirstOrDefaultAsync<RaceCompleteResponseDTO>(selectQuery);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetRaceInProgress in CompetitionRepository");
                throw;
            }
        }
        public async Task UpdateStatusRaceAsync(Guid id)
        {
            try
            {
                var updateQuery = @"UPDATE Races
                                    SET Status = 'InProgress'
                                    WHERE Id = @Id";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateStatusRace in CompetitionRepository");
                throw;
            }
        }

        public async Task<List<RaceResponseDTO>> GetRacesAsync()
        {
            try
            {
                var selectQuery = @"SELECT C.Name AS NameCircuit, C.Country AS CountryCircuit, R.[Round], S.[Year], C.LapsNumber 
                                    FROM Races R
                                    JOIN Circuits C ON R.CircuitId = C.Id
                                    JOIN Season S ON R.SeasonId = S.Id";
                                   
                var calendar = await _connection.QueryAsync<RaceResponseDTO>(selectQuery);
                return calendar.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetRacesAsyn in CompetitionRepository");
                throw;
            }
        }
        public async Task<RaceResponseDTO?> GetRaceByIdAsync(Guid Id)
        {
            try
            {
                var selectQuery = @"SELECT C.Name AS NameCircuit, C.Country AS CountryCircuit, R.[Round], S.[Year], C.LapsNumber 
                                    FROM Races R
                                    JOIN Circuits C ON R.CircuitId = C.Id
                                    JOIN Season S ON R.SeasonId = S.Id
                                    WHERE R.Id = @Id";
                return await _connection.QueryFirstOrDefaultAsync<RaceResponseDTO>(selectQuery, new { Id = Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetRaceByIdAsync in CompetitionRepository");
                throw;
            }
        }

        public async Task<RaceWithCircuitResponseDTO?> GetRaceWithCircuitAsync()
        {
            try
            {
                
                var selectQuery = @"SELECT R.Id, R.SeasonId, R.[Round], R.[Status], R.T1, R.T2, R.T3, R.Qualifier,
                                   C.Id AS CircuitId, C.[Name], C.Country, C.LapsNumber, C.IsActive
                                   FROM Races R
                                  JOIN Circuits C ON R.CircuitId = C.Id
                                  WHERE R.[Status] = 'InProgress'";

                
                var result = await _connection.QueryAsync<RaceWithCircuitResponseDTO, CircuitCompleteResponseDTO, RaceWithCircuitResponseDTO>(
                    selectQuery,
                    static (race, circuit) =>
                    {
                        race.Circuit = circuit;
                        return race;
                    },
                    splitOn: "CircuitId" 
                );
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetRaceWithCircuitAsync in CompetitionRepository");
                throw;
            }
        }

        public async Task UpdateRaceT1Async()
        {
            try
            {
                var updateQuery = @"UPDATE Races
                                  SET T1 = 1
                                  WHERE [Status] = 'InProgress'";

                await _connection.ExecuteAsync(updateQuery);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateRaceT1Async in CompetitionRepository");
                throw;
            }
        }


        public async Task UpdateRaceT2Async()
        {
            try
            {
                var updateQuery = @"UPDATE Races
                                  SET T2 = 1
                                  WHERE [Status] = 'InProgress'";

                await _connection.ExecuteAsync(updateQuery);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateRaceT2Async in CompetitionRepository");
                throw;
            }
        }

        public async Task UpdateRaceT3Async()
        {
            try
            {
                var updateQuery = @"UPDATE Races
                                  SET T3 = 1
                                  WHERE [Status] = 'InProgress'";

                await _connection.ExecuteAsync(updateQuery);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateRaceT3Async in CompetitionRepository");
                throw;
            }
        }

        public async Task UpdateRaceQualifierAsync()
        {
            try
            {
                var updateQuery = @"UPDATE Races
                                  SET Qualifier = 1
                                  WHERE [Status] = 'InProgress'";

                await _connection.ExecuteAsync(updateQuery);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateRaceQualifierAsync in CompetitionRepository");
                throw;
            }
        }

        public async Task UpdateRaceRaceAsync()
        {
            try
            {
                var updateQuery = @"UPDATE Races
                                  SET Race = 1
                                  WHERE [Status] = 'InProgress'";

                await _connection.ExecuteAsync(updateQuery);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateRaceRaceAsync in CompetitionRepository");
                throw;
            }
        }

        public async Task<List<DriverStandingResponseDTO>> GetDriverStandingAsync()
        {
            try
            {
                var selectQuery = @"SELECT DriverId, DriverName, Points
                                    FROM DriverStanding
                                    WHERE SeasonId = (SELECT Id FROM Season WHERE IsActive = 1)
                                    ORDER BY Points DESC";
                var driverStandings = await _connection.QueryAsync<DriverStandingResponseDTO>(selectQuery);

                return driverStandings.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetDriverStanding in CompetitionRepository");
                throw;
            }
        }

        public async Task<List<TeamStandingResponseDTO>> GetTeamStandingAsync()
        {
            try
            {
                var selectQuery = @"SELECT TeamId, TeamName, Points
                                    FROM TeamStanding
                                    WHERE SeasonId = (SELECT Id FROM Season WHERE IsActive = 1)
                                    ORDER BY Points DESC";
                var teamStandings = await _connection.QueryAsync<TeamStandingResponseDTO>(selectQuery);
                return teamStandings.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTeamStanding in CompetitionRepository");
                throw;
            }
        }

    }
}
