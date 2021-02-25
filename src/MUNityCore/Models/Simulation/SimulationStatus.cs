using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.Simulation
{
    public class SimulationStatus
    {
        public int SimulationStatusId { get; set; }

        public string StatusText { get; set; }

        public DateTime StatusTime { get; set; }

        public Simulation Simulation { get; set; }
    }
}
