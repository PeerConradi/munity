using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Simulation
{
    /// <summary>
    /// The IUserItem Interface is for any kind of SimulationUser Implementation
    /// </summary>
    public interface IUserItem
    {
        /// <summary>
        /// The simulation user Id.
        /// </summary>
        int SimulationUserId { get; set; }

        /// <summary>
        /// The display name of the user.
        /// </summary>
        string DisplayName { get; set; }

        /// <summary>
        /// The current role this user has picked.
        /// </summary>
        int RoleId { get; set; }

        /// <summary>
        /// Returns of the user is connected or not. This is known if there is an active Hub Connection.
        /// </summary>
        bool IsOnline { get; set; }
    }
}
