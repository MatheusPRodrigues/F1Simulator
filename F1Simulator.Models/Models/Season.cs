using System;
using System.Collections.Generic;
using System.Text;

namespace F1Simulator.Models.Models
{
    public class Season
    {
        public Guid Id { get; private set; }
        public int Year { get; private set; }
        public bool IsActive { get; private set; }

        public Season(int year)
        {
            Id = Guid.NewGuid();
            Year = year;
            IsActive = true;
        }
    }
}
