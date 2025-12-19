using F1Simulator.Models.DTOs.TeamManegementService.BossDTO;
using F1Simulator.Models.Models.TeamManegement;
using F1Simulator.Models.Models.TeamManegementService;
using F1Simulator.TeamManagementService.Data;
using F1Simulator.TeamManagementService.Repositories;
using F1Simulator.TeamManagementService.Repositories.Interfaces;
using F1Simulator.TeamManagementService.Services.Interfaces;

namespace F1Simulator.TeamManagementService.Services
{
    public class BossService : IBossService
    {
        private readonly ILogger<Boss> _logger;
        private readonly IBossRepository _bossRepository;
        private readonly ITeamRepository _teamRepository;

        public BossService(ILogger<Boss> logger, IBossRepository bossRepository, ITeamRepository teamRepository)
        {
            _logger = logger;
            _bossRepository = bossRepository;
            _teamRepository = teamRepository;
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

                var team = await _teamRepository.GetTeamByIdAsync(teamId);

                if (team is null)
                    throw new Exception("Team does not exist");


                var bossCount = await _bossRepository.GetBossByTeamCountAsync(teamId);
                if (bossCount >= 2)
                    throw new Exception("Already have 2 bosses in the team, that's the limit!");

                var boss = new Boss
                {
                    BossId = Guid.NewGuid(),
                    TeamId = teamId,
                    FirstName = bossDto.FirstName,
                    LastName = bossDto.LastName,
                    Age = bossDto.Age
                };

                await _bossRepository.CreateBossAsync(boss);
            }
            catch(Exception ex)
            {
                _logger.LogError($"An error occurred while creating the boss: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<List<BossResponseDTO>> GetAllBossesAsync()
        {
            try
            {
                var boss = await _bossRepository.GetAllBossesAsync();
                return boss.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting the boss list: {ex.Message}", ex);
                throw;
            }
        }
        public async Task<List<BossWithTeamDTO>> GetBossesWithTeamAsync()
        {
            try
            {
                var boss = await _bossRepository.GetBossesWithTeamAsync();
                return boss.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting the boss list: {ex.Message}", ex);
                throw;
            }
        }
    }
}
