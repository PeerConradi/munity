using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Simulation
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

    /// <summary>
    /// The different modes the lobby phase can have.
    /// </summary>
    public enum LobbyModes
    {
        /// <summary>
        /// Allows to Join the game with a role token and will then give
        /// you the role.
        /// </summary>
        WithRoleKey,
        /// <summary>
        /// You can join the lobby and pick a role
        /// </summary>
        PickRole,
        /// <summary>
        /// Allow everyone inside the Lobby to create a role
        /// </summary>
        CreateAnyRole,
        /// <summary>
        /// the lobby is closed you cannot join.
        /// </summary>
        Closed
    }
}
