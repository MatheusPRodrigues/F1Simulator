using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.Models
{
    public class Circuit
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Country { get; private set; }
        public int LapsNumber { get; private set; }
        public bool IsActive { get; private set; }
       
        public Circuit(string name, string country, int lapsNumber, bool isActive)
        {
            Id = Guid.NewGuid();
            Name = name;
            Country = country;
            LapsNumber = lapsNumber;
            IsActive = isActive;
        }
    }
}
