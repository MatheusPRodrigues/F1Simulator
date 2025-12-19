using F1Simulator.Models.Enums.CompetitionService;
using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.Models
{
    public class Race
    {
        public Guid Id { get; private set; }
        public Guid SeasonId { get; private set; }
        public Guid CircuitId { get; private set; }
        public int Round { get; private set; }
        public string Status { get; private set; }
        public bool T1 { get; private set; }
        public bool T2 { get; private set; }
        public bool T3 { get; private set; }
        public bool Qualifier { get; private set; }
        public bool RaceFinal { get; private set; }


        public Race(Guid seasonId, Guid circuitId, int round)
        {
            Id = Guid.NewGuid();
            SeasonId = seasonId;
            CircuitId = circuitId;
            Round = round;
            Status = RaceStatus.Pending.ToString();
            T1 = false;
            T2 = false;
            T3 = false;
            Qualifier = false;
            RaceFinal = false;
        }
    }
}
