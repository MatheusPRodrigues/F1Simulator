namespace F1Simulator.Models.Models.RaceControlService
{
    public class DriverRace
    {
        public Guid DriverId { get; private set; }
        public string DriverName { get; private set; }
        public Guid TeamId { get; private set; }
        public string TeamName { get; private set; }
        public int Position { get; private set; }
        public int Pontuation { get; private set; }

        public DriverRace
        (
            Guid driverId,
            string driverName,
            Guid teamId,
            string teamName,
            int position,
            int pontuation
        )
        {
            DriverId = driverId;
            DriverName = driverName;
            TeamId = teamId;
            TeamName = teamName;
            Position = position;
            Pontuation = pontuation;
        }
    }
}
