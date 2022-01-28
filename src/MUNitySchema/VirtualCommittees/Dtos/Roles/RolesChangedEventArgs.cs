using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.VirtualCommittees.Dtos
{
    /// <summary>
    /// Event arguments for when the roles of a simulation have changed.
    /// This could mean that a role has been added, removed or been renamed.
    /// It will contain the List of new Roles.
    /// </summary>
    public class RolesChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The Id of the Simulation that is effected by the changes
        /// </summary>
        public int SimulationId { get; private set; }

        /// <summary>
        /// The current or new roles
        /// </summary>
        public IEnumerable<SimulationRoleDto> Roles { get; private set; }

        /// <summary>
        /// Create event arguments with all given parameters
        /// </summary>
        /// <param name="simulationId"></param>
        /// <param name="roles"></param>
        public RolesChangedEventArgs(int simulationId, IEnumerable<SimulationRoleDto> roles)
        {
            this.SimulationId = simulationId;
            this.Roles = roles;
        }
    }
}
