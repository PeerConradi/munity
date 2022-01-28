using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.VirtualCommittees.Dtos
{
    /// <summary>
    /// Select a new Role for a given user.
    /// </summary>
    public class SetUserSimulationRole : SimulationRequest
    {
        /// <summary>
        /// The SimulationUserId that the Roles should be changed of.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The RoleId that the user should select
        /// <seealso cref="SimulationRoleDto"/>
        /// </summary>
        public int RoleId { get; set; }
    }
}
