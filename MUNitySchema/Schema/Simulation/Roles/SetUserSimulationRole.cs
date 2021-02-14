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
        /// <summary>
        /// The SimulationUserId that the Roles should be changed of.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// The RoleId that the user should select
        /// <seealso cref="SimulationRoleDto"/>
        /// </summary>
        public int RoleId { get; set; }
    }
}
