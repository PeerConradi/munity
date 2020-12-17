using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Schema.Response.Simulation
{
    public class SimulationResponses
    {
        [Obsolete("New Simulation implementation to be done!")]
        public struct SimulationList
        {
            public int SimulationId { get; set; }

            public string Name { get; set; }

            public bool UsingPassword { get; set; }
        }
    }
}
