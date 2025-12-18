using F1Simulator.Models.Models.TeamManegementService;
using F1Simulator.Models.DTOs.TeamManegementService.BossDTO;
using F1Simulator.TeamManagementService.Data;
using F1Simulator.TeamManagementService.Repositories;
namespace F1Simulator.TeamManagementService.Services
{
    public class BossService
    {
        private readonly TeamManagementServiceConnection _connection;
        private readonly ILogger<BossRequestDTO> _logger;
        private readonly BossRepository _bossRepository;
        public BossService(ILogger<BossRequestDTO> logger, TeamManagementServiceConnection connection, BossRepository bossRepository)
        {
            _logger = logger;
            _bossRepository = bossRepository;
            _connection = connection;
        }

        public async Task<int> GetBossByTeamCountAsync(string teamId)
        {
            try
            {
                return await _bossRepository.GetBossByTeamCountAsync(Guid.Parse(teamId));
            }
            catch(Exception ex)
            {
                _logger.LogError($"An error occurred while getting count the boss in the team: {ex.Message}", ex);
                throw;
            }
        }

        public async Task CreateBossAsync(BossRequestDTO bossDto)
        {
            try
            {
                if (!Guid.TryParse(bossDto.TeamId, out var teamId))
                    throw new Exception("Invalid team id!");

                var bossCount = await _bossRepository.GetBossByTeamCountAsync(teamId);
                if (bossCount > 2)
                    throw new Exception("Already have 2 bosses in the team, that's the limit!");

                var boss = new Boss
                {
                    BossId = Guid.NewGuid(),
                    TeamId = teamId,
                    FirstName = bossDto.FirstName,
                    FullName = bossDto.FullName,
                    Age = bossDto.Age,
                    IsActive = true
                };
            }
            catch(Exception ex)
            {
                _logger.LogError($"An error occurred while creating the boss: {ex.Message}", ex);
                throw;
            }
        }
    }
}
