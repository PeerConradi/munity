using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Simulation
{
    /// <summary>
    /// The schema you will get when asking for a simulation with a valid token.
    /// </summary>
    public class SimulationDto
    {
        /// <summary>
        /// the id of the simulation.
        /// </summary>
        public int SimulationId { get; set; }

        /// <summary>
        /// The name of the simulation.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The current phase of the simulation.
        /// </summary>
        public GamePhases Phase { get; set; }

        /// <summary>
        /// The roles of the simulation.
        /// </summary>
        public List<SimulationRoleDto> Roles { get; set; }

        /// <summary>
        /// The users of the simulation.
        /// </summary>
        public List<SimulationUserDefaultDto> Users { get; set; }
    }
}
