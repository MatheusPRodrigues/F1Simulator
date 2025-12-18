namespace F1Simulator.Models.Models.RaceControlService
{
    public class DriverToQueueDTO
    {
        public string DriverId { get; init; }
        public string DriverName { get; init; }
        public double Handicap { get; set; }
        public int DriverExp { get; init; }
        public string TeamId { get; init; }
        public string TeamName { get; init; }
        public string EnginneringAId { get; init; }
        public string EnginneringPId { get; init; }
        public string CarId { get; init; }
        public double Ca { get; set; }
        public double Cp { get; set; }
    }
}
