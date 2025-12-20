using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.Models
{
    public class DriverStanding
    {
        public Guid Id { get; private set; }
        public Guid DriverId { get; private set; }
        public Guid SeasonId { get; private set; }
        public Guid TeamId { get; private set; }
        public string DriverName { get; set; }
        public int Position { get; private set; }
        public decimal Points { get; private set; }


        public DriverStanding(Guid driverId, Guid seasonId, Guid teamId, int position, decimal points, string driverName)
        {
            Id = Guid.NewGuid();
            DriverId = driverId;
            SeasonId = seasonId;
            TeamId = teamId;
            Position = position;
            Points = points;
            DriverName = driverName;
        }
    }  
}
