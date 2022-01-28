using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.VirtualCommittees.Dtos
{
    /// <summary>
    /// Event Args when a user has been given a new role
    /// </summary>
    public class UserRoleChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The id of the simulation that the changes had been made to
        /// </summary>
        public int SimulationId { get; private set; }

        /// <summary>
        /// The SimulationUserId of the user that is affected by this changed
        /// </summary>
        public int UserId { get; private set; }

        /// <summary>
        /// The id of the new role that has been selected.
        /// </summary>
        public int RoleId { get; private set; }

        /// <summary>
        /// Creates a new UserRoleChanged EventArgs with all needed parameters
        /// </summary>
        /// <param name="simulationId"></param>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        public UserRoleChangedEventArgs(int simulationId, int userId, int roleId)
        {
            this.SimulationId = simulationId;
            this.UserId = userId;
            this.RoleId = roleId;
        }
    }
}
