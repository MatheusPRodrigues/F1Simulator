using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace F1Simulator.Models.Models.RaceControlService
{
    public class RaceControl
    {
        public ObjectId Id { get; private set; }
        public Guid RaceId { get; private set; }
        public int Round { get; private set; }
        public int Season { get; private set; }
        public CircuitRace Circuit { get; private set; }
        public List<DriverQualifier> GridQualifier { get; private set; }
        public List<DriverRace> GridRace { get; private set; }

        public RaceControl
        (
            Guid raceId,
            int round,
            int season,
            CircuitRace circuit,
            List<DriverQualifier> gridQualifier
        )
        {
            Id = ObjectId.GenerateNewId();
            Round = round;
            Season = season;
            Circuit = circuit;
            GridQualifier = gridQualifier;
            GridRace = new List<DriverRace>();
        }

        public RaceControl
        (
            ObjectId id,
            Guid raceId,
            int round,
            int season,
            CircuitRace circuit,
            List<DriverQualifier> gridQualifier,
            List<DriverRace> gridRace
        )
        {
            Id = id;
            RaceId = raceId;
            Round = round;
            Season = season;
            Circuit = circuit;
            GridQualifier = gridQualifier;
            GridRace = gridRace;
        }
    }
}
