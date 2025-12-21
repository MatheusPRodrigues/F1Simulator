namespace F1Simulator.Models.Models.TeamManegement
{
    public class Car
    {
        public Guid CarId { get; set; }
        public Guid TeamId { get; set; }
        public string Model { get; set; }
        public double WeightKg { get; set; }
        public int Speed { get; set; }
        public double Ca { get; set; }
        public double Cp { get; set; }

        public Car(Guid teamId, string model, double weightKg, int speed, double ca, double cp)
        {
            CarId = Guid.NewGuid();
            TeamId = teamId;
            Model = model;
            WeightKg = weightKg;
            Speed = speed;
            Ca = ca;
            Cp = cp;
        }
    }
}
