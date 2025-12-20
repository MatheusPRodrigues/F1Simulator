using F1Simulator.Models.DTOs.TeamManegementService.BossDTO;

namespace F1Simulator.TeamManagementService.Services.Interfaces
{
    public interface IBossService
    {
        Task<int> GetBossByTeamCountAsync(string teamId);
        Task CreateBossAsync(BossRequestDTO bossDto);
        Task<List<BossResponseDTO>> GetAllBossesAsync();
        Task<List<BossWithTeamDTO>> GetBossesWithTeamAsync();
        Task<int> GetAllBossesCountAsync();
    }
}
