using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Simulation
{
    /// <summary>
    /// The Auth Schema sent by the API when given a token.
    /// </summary>
    public class SimulationAuthSchema
    {
        /// <summary>
        /// The internal user id used to identify the user inside the simulation.
        /// </summary>
        public int SimulationUserId { get; set; }

        /// <summary>
        /// Can the user create a new role. This is mostly used by now to identify if the user is an administrator.
        /// </summary>
        public bool CanCreateRole { get; set; }

        /// <summary>
        /// Can the user select a role on his/her own.
        /// </summary>
        public bool CanSelectRole { get; set; }

        /// <summary>
        /// Can the user edit the resolution that is linked to this simulation.
        /// </summary>
        public bool CanEditResolution { get; set; }

        /// <summary>
        /// Can the user edit the list of speakers that is linked to this simulation.
        /// </summary>
        public bool CanEditListOfSpeakers { get; set; }
    }
}
