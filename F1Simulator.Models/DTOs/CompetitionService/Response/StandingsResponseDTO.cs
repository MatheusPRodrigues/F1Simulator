namespace F1Simulator.Models.DTOs.CompetitionService.Response
{
    public class StandingsResponseDTO
    {
        public List<DriverStandingResponseWhitPositionDTO> DriverStandings { get; set; } = new List<DriverStandingResponseWhitPositionDTO>();
        public List<TeamStandingResponseWhitPositionDTO> TeamStandings { get; set; } = new List<TeamStandingResponseWhitPositionDTO>();
    }
}
