using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.VirtualCommittee.Dtos.Roles
{
    /// <summary>
    /// User has connected event args. This will be used by the socket in future implementations.
    /// </summary>
    public class UserConnectedEventArgs : EventArgs
    {
        /// <summary>
        /// Id of the simulation that a user has connected to
        /// </summary>
        public int SimulationId { get; set; }

        /// <summary>
        /// The id of the user that has connected
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The display name of the user that has connected.
        /// </summary>
        public string DisplayName { get; set; }
    }
}
