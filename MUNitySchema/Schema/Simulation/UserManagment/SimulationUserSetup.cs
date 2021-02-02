using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Simulation
{
    /// <summary>
    /// User Package for the administrators that can see and edit the users.
    /// </summary>
    public class SimulationUserSetup : SimulationUserItem
    {

        /// <summary>
        /// The Public Id of the user.
        /// </summary>
        public string PublicId { get; set; }

        /// <summary>
        /// The password of the user.
        /// </summary>
        public string Password { get; set; }
    }
}
