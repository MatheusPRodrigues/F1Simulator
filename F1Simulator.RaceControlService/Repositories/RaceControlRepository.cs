using F1Simulator.Models.Models.RaceControlService;
using F1Simulator.RaceControlService.Repositories.Interfaces;
using F1Simulator.Utils.DatabaseConnectionFactory;
using MongoDB.Driver;

namespace F1Simulator.RaceControlService.Repositories
{
    public class RaceControlRepository : IRaceControlRepository
    {
        private readonly IMongoDatabase _mongoDatabase;
        private readonly IMongoCollection<RaceControl> _raceControlCollection;

        public RaceControlRepository(IDatabaseConnection<IMongoDatabase> mongoDatabase)
        {
            _mongoDatabase = mongoDatabase.Connect();
            _raceControlCollection = _mongoDatabase.GetCollection<RaceControl>("RaceControll");
        }

        public async Task<List<DriverQualifier>> GetDriverQualiersByRaceIdAsync(Guid raceId)
        {
            try
            {
                var race = await _raceControlCollection.FindAsync<RaceControl>(r => r.RaceId == raceId).Result.FirstOrDefaultAsync();
                return race.GridQualifier;
            }
            catch (MongoException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<RaceControl> GetRaceByIdAsync(Guid raceId)
        {
            try
            {
                return await _raceControlCollection.FindAsync(r => r.RaceId == raceId).Result.FirstOrDefaultAsync();
            }
            catch (MongoException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<RaceControl>> GetRacesBySeasonYearAsync(int year)
        {
            try
            {
                return await _raceControlCollection.FindAsync(r => r.Season == year).Result.ToListAsync();
            }
            catch (MongoException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task InsertRaceControlRegisterAsync(RaceControl raceControl)
        {
            try
            {
                await _raceControlCollection.InsertOneAsync(raceControl);
            }
            catch (MongoException ex)
            {
                throw;
            } 
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task ReplaceDriverRaceAsync(RaceControl raceControl)
        {
            try
            {
                await _raceControlCollection.ReplaceOneAsync(r => r.RaceId == raceControl.RaceId, raceControl);
            }
            catch (MongoException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
