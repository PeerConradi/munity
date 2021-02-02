using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Simulation
{
    public class RolesChangedEventArgs : EventArgs
    {
        public int SimulationId { get; private set; }

        public IEnumerable<SimulationRoleItem> Roles { get; private set; }

        public RolesChangedEventArgs(int simulationId, IEnumerable<SimulationRoleItem> roles)
        {
            this.SimulationId = simulationId;
            this.Roles = roles;
        }
    }
}
