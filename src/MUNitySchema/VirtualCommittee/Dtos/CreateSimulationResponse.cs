using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.VirtualCommittee.Dtos
{
    /// <summary>
    /// Response of creating a Simulation. The creation will automatically create a first user
    /// this response will return the Id, password and a token for this first user.
    /// </summary>
    public class CreateSimulationResponse
    {
        /// <summary>
        /// The Id of the Simulation
        /// </summary>
        public int SimulationId { get; set; }

        /// <summary>
        /// The name of the simulation (Display Name)
        /// </summary>
        public string SimulationName { get; set; }

        /// <summary>
        /// The user of the first user that was added. This is the owner and the user has the owner auth of this
        /// simulation.
        /// </summary>
        public string FirstUserId { get; set; }

        /// <summary>
        /// The inital password of the owner.
        /// </summary>
        public string FirstUserPassword { get; set; }

        /// <summary>
        /// The user token of the owner.
        /// </summary>
        public string FirstUserToken { get; set; }
    }
}
