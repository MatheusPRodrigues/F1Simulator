using F1Simulator.Models.Models.RaceControlService;

namespace F1Simulator.RaceControlService.Repositories.Interfaces
{
    public interface IRaceControlRepository
    {
        Task InsertRaceControlRegisterAsync(RaceControl raceControl);
        Task<List<DriverQualifier>> GetDriverQualiersByRaceId(Guid raceId);
        Task<RaceControl> GetRaceByIdAsync(Guid raceId);
        Task ReplaceDriverRaceAsync(RaceControl raceControl);
        Task<List<RaceControl>> GetRacesBySeasonYearAsync(int year);
    }
}
