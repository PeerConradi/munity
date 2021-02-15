using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Simulation
{
    /// <summary>
    /// A response you get when Joining a Simulation.
    /// </summary>
    public class SimulationTokenResponse
    {
        /// <summary>
        /// The Id of the simulation.
        /// </summary>
        public int SimulationId { get; set; }

        /// <summary>
        /// The name of the simulation.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The token of the simulation. You will be identified with this token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// The user pin. This is used when your token is lost and you want to claim that you are a specific SimulationUser
        /// later.
        /// </summary>
        public string Pin { get; set; }
    }
}
