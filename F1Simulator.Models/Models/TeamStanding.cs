namespace F1Simulator.Models.Models
{
    public class TeamStanding
    {
        public Guid Id { get; private set; }
        public Guid SeasonId { get; private set; }
        public Guid TeamId { get; private set; }
        public string TeamName { get; private set; }
        public int Points { get; set; }

        public TeamStanding(Guid seasonId, Guid teamId, int points, string teamName)
        {
            Id = Guid.NewGuid();
            SeasonId = seasonId;
            TeamId = teamId;
            Points = points;
            TeamName = teamName;
        }
    }
}
