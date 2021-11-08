using MUNity.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Simulation
{
    /// <summary>
    /// The schema the API will give you when asking for a lis of simulations.
    /// </summary>
    public class SimulationListItemDto
    {
        /// <summary>
        /// The id of the simulation.
        /// </summary>
        public int SimulationId { get; set; }

        /// <summary>
        /// The name of the simulation.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Is the simulation using a password.
        /// </summary>
        public bool UsingPassword { get; set; }

        /// <summary>
        /// The current phase of the simulation.
        /// </summary>
        public GamePhases Phase { get; set; }
    }
}
