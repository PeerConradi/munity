using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Simulation
{
    /// <summary>
    /// A wrapper class containing the enums used by the simulation.
    /// </summary>
    public static class SimulationEnums
    {
        /// <summary>
        /// The roles a user can take when inside a simulation / virtual committee.
        /// </summary>
        public enum RoleTypes
        {
            /// <summary>
            /// A spectator role that cannot interact with the simulation but will be displayed as spectator.
            /// For example press, spectators another committee that is visiting this committee etc.
            /// </summary>
            Spectator,
            /// <summary>
            /// The chairman role that can edit the list of speakers and resolution and can interact with requests of other roles.
            /// </summary>
            Chairman,
            /// <summary>
            /// a delegate role that is part of the committee.
            /// </summary>
            Delegate,
            /// <summary>
            /// A moderator role that can interact with all other users in this simulation by having higher powers than any other role
            /// and is allowed to kick users.
            /// </summary>
            Moderator,
            /// <summary>
            /// A non government organization that has some ability to interact with the simulation but not as much as a delegate (cant vote etc.)
            /// </summary>
            Ngo
        }

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
}
