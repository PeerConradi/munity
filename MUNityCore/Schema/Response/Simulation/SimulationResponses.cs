using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Schema.Response.Simulation
{
    public class SimulationResponses
    {
        public struct SimulationList
        {
            public int SimulationId { get; set; }

            public string Name { get; set; }

            public bool UsingPassword { get; set; }
        }

        public struct SimulationCreatedResponse
        {
            public int SimulationId { get; set; }

            public string OwnerKey { get; set; }
        }
    }
}
