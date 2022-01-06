using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MUNity.VirtualCommittee.Dtos
{
    /// <summary>
    /// Every Request that is happening inside the Simulation must be authenticated.
    /// This abstraction will ensure that a Token is passed.
    /// </summary>
    public class SimulationRequest
    {
        /// <summary>
        /// The token of the current logged in SimulationUser.
        /// </summary>
        [Required]
        public string Token { get; set; }

        /// <summary>
        /// The id of the Simulation that changes should be made in.
        /// </summary>
        [Required]
        public int SimulationId { get; set; }
    }
}
