namespace F1Simulator.Models.DTOs.RaceControlService
{
    public class DriverToRaceDTO
    {
        public string DriverId { get; init; }
        public string DriverName { get; init; }
        public double Handicap { get; set; }
        public double DriverExp { get; init; }
        public string TeamId { get; init; }
        public string TeamName { get; init; }
        public string EnginneringAId { get; init; }
        public string EnginneringPId { get; init; }
        public string CarId { get; init; }
        public double Ca { get; set; }
        public double Cp { get; set; }
    }
}
