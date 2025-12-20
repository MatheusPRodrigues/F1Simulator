namespace F1Simulator.Models.Models.TeamManegementService
{
    public class Boss
    {
        public Guid BossId { get; set; }
        public Guid TeamId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }
}
