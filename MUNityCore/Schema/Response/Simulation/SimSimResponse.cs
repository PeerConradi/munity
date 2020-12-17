using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Schema.Response.Simulation
{
    public class SimSimResponse
    {
        public int SimulationId { get; set; }

        public string Name { get; set; }

        public IEnumerable<SimulationRoleResponse> Roles { get; set; }

        public SimSimResponse(Models.Simulation.Simulation simulation)
        {
            this.SimulationId = simulation.SimulationId;
            this.Name = simulation.Name;
            this.Roles = simulation.Roles.Cast<SimulationRoleResponse>();
        }

        public static implicit operator SimSimResponse(Models.Simulation.Simulation simulation)
        {
            return new SimSimResponse(simulation);
        }
    }
}
