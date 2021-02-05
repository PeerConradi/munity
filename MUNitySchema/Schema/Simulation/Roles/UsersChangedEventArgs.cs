using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Simulation
{
    /// <summary>
    /// Updates the user List
    /// </summary>
    public class UsersChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The SimulationId that the user list has changed at
        /// </summary>
        public int SimulationId { get; set; }

        /// <summary>
        /// The new List of users.
        /// </summary>
        public List<SimulationUserItem> Users { get; set; }
    }
}
