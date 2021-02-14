using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Simulation
{
    /// <summary>
    /// A role of a simulation/virtual committee.
    /// </summary>
    public class SimulationRoleDto
    {
        
        /// <summary>
        /// The id of the role.
        /// </summary>
        public int SimulationRoleId { get; set; }

        /// <summary>
        /// The name of the role
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The type of role like Chairman, Delegate etc.
        /// </summary>
        public RoleTypes RoleType { get; set; }

        /// <summary>
        /// The iso of the role that could be used to find an icon for a role
        /// </summary>
        public string Iso { get; set; }

        /// <summary>
        /// Display Names of users that have taking this role.
        /// </summary>
        public List<string> Users { get; set; }
    }
}
