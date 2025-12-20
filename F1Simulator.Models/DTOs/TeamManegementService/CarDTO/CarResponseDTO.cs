namespace F1Simulator.Models.DTOs.TeamManegementService.CarDTO
{
    public class CarResponseDTO
    {
        public Guid CarId { get; init; }
        public Guid TeamId { get; init; }
        public string Model { get; init; }
        public double WeightKg { get; init; }
        public int Speed { get; init; }
        public double Ca { get; init; }
        public double Cp { get; init; }
    }
}
