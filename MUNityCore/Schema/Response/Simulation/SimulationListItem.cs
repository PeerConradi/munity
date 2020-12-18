using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Schema.Response.Simulation
{
    public class SimulationListItem
    {
        public int SimulationId { get; set; }

        public string Name { get; set; }

        public bool UsingPassword { get; set; }

        public Models.Simulation.Simulation.GamePhases Phase { get; set; }

        public SimulationListItem(Models.Simulation.Simulation simulation)
        {
            this.SimulationId = simulation.SimulationId;
            this.Name = simulation.Name;
            this.UsingPassword = !string.IsNullOrEmpty(simulation.Password);
            this.Phase = simulation.Phase;
        }

        public static implicit operator SimulationListItem(Models.Simulation.Simulation simulation)
        {
            return new SimulationListItem(simulation);
        }
    }
}
