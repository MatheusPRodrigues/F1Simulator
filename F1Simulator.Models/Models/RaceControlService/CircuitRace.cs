using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.Models.RaceControlService
{
    public class CircuitRace
    {
        public Guid CircuitId { get; private set; }
        public string CircuitName { get; private set; }
        public string Country { get; private set; }
        public int LapsNumber { get; private set; }

        public CircuitRace
        (
            Guid circuitId,
            string circuitName,
            string country,
            int lapsNumber
        )
        {
            CircuitId = circuitId;
            CircuitName = circuitName;
            Country = country;
            LapsNumber = lapsNumber;
        }
    }
}
