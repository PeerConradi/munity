using System;
using System.Collections.Generic;
using System.Text;

namespace MUNityBase
{
    /// <summary>
    /// The different phases of the simulation.
    /// </summary>
    public enum GamePhases
    {
        /// <summary>
        /// The simulation is marked as offline. To login or start any other phase you have to have a valid token that
        /// is marked as admin or will have to type in the admin password to become admin.
        /// </summary>
        Offline,
        /// <summary>
        /// The Simulation is currently on the lobby phase. This means the roles and different countries are being set and
        /// users become different roles or can pick roles. The simulation has not started yet.
        /// </summary>
        Lobby,
        /// <summary>
        /// The simulation has started and is active. Joining is maybe still allowed.
        /// </summary>
        Online
    }
}
