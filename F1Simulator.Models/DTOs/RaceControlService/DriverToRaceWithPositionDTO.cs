using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.DTOs.RaceControlService
{
    public class DriverToRaceWithPositionDTO : DriverToRaceDTO
    {
        public int Position { get; init; }
    }
}
