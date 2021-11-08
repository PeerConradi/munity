using MUNity.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Simulation
{
    public class SimulationInfo
    {
        public int SimulationId { get; set; }

        public string Name { get; set; }

        public GamePhases Phase { get; set; }

        public int SlotCount { get; set; }

        public int RolesCount { get; set; }
    }
}
