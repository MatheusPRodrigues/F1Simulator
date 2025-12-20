using F1Simulator.Models.DTOs.TeamManegementService.BossDTO;
using F1Simulator.Models.Models.TeamManegementService;

namespace F1Simulator.TeamManagementService.Repositories.Interfaces
{
    public interface IBossRepository
    {
        Task<int> GetBossByTeamCountAsync(Guid teamId);
        Task CreateBossAsync(Boss boss);
        Task<IEnumerable<BossResponseDTO>> GetAllBossesAsync();
        Task<IEnumerable<BossWithTeamDTO>> GetBossesWithTeamAsync();
        Task<int> GetAllBossesCountAsync();
    }
}
