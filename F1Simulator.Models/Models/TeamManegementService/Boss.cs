using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.Models.TeamManegementService
{
    public class Boss
    {
        public Guid BossId { get; set; }
        public Guid TeamId { get; set; }
        public string FirstName { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
