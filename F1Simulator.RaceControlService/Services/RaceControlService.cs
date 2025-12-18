using F1Simulator.RaceControlService.Messaging;
using F1Simulator.RaceControlService.Repositories.Interfaces;
using F1Simulator.RaceControlService.Services.Interfaces;

namespace F1Simulator.RaceControlService.Services
{
    public class RaceControlService : IRaceControlService
    {
        private readonly IPublishService _messageService;
        private readonly IRaceControlRepository _raceControlRepository;

        public RaceControlService(
            IPublishService messageService,
            IRaceControlRepository raceControlRepository
        )
        {
            _messageService = messageService; 
            _raceControlRepository = raceControlRepository;
        }

        public Task ExecuteQualifierSectionAsync(string raceId)
        {
            throw new NotImplementedException();
        }

        public Task ExecuteRaceSectionAsync(string raceId)
        {
            throw new NotImplementedException();
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
    }
}
