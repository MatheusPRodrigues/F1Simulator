using F1Simulator.Utils.DatabaseConnectionFactory.Config;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace F1Simulator.Utils.DatabaseConnectionFactory.Connections
{
    public class MongoConnection : IDatabaseConnection<IMongoDatabase>
    {
        private readonly MongoClient _client;
        private readonly IOptions<MongoDBSettings> _settings;
        public MongoConnection(IOptions<MongoDBSettings> settings)
        {
            _settings = settings;
            _client = new MongoClient(_settings.Value.ConnectionURI);
        }

        public IMongoDatabase Connect()
        {
            return _client.GetDatabase(_settings.Value.DatabaseName);
        }
    }
}
