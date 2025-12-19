using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.Models.RaceControlService
{
    public class DriverQualifier
    {
        public Guid DriverId { get; private set; }
        public string DriverName { get; private set; }
        public int Position { get; private set; }

        public DriverQualifier
        (
            Guid driverId,
            string driverName,
            int position
        )
        {
            DriverId = driverId;
            DriverName = driverName;
            Position = position;
        }
    }
}
