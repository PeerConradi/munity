using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Simulation
{
    public class SimulationStatusDto
    {
        public int SimulationStatusId { get; set; }

        public string StatusText { get; set; }

        public DateTime StatusTime { get; set; }
    }
}
