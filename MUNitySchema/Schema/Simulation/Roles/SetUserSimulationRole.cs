using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Simulation
{
    /// <summary>
    /// Select a new Role for a given user.
    /// </summary>
    public class SetUserSimulationRole : SimulationRequest
    {
        public string UserId { get; set; }

        public int RoleId { get; set; }
    }
}
