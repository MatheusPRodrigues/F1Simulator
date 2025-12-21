namespace F1Simulator.Models.DTOs.TeamManegementService.DriverDTO
{
    public class DriverRequestDTO
    {
        public Guid TeamId { get; init; }
        public Guid CarId { get; init; }
        public string FirstName { get; init; }
        public string FullName { get; init; }
        public int DriverNumber { get; init; }
        public double WeightKg { get; init; }
    }
}
