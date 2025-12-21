namespace F1Simulator.Models.DTOs.RaceControlService
{
    public class DriverToPublishDTO
    {
        public string DriverId { get; init; }
        public string DriverName { get; init; }
        public string TeamId { get; init; }
        public string TeamName { get; init; }
        public int Pontuation { get; init; }
    }
}
