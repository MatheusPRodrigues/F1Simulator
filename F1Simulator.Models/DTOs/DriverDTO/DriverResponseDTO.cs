namespace F1Simulator.Models.DTOs.DriverDTO
{
    public class DriverResponseDTO
    {
        public Guid DriverId { get; init; }
        public int DriverNumber { get; init; }
        public string TeamId { get; init; }
        public string CarId { get; init; }
        public string FirstName { get; init; }
        public string FullName { get; init; }
        public string NameAcronym { get; init; }
        public double WeightKg { get; init; }
        public double HandiCap { get; init; }
    }
}
