using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MUNity.Schema.Simulation
{
    /// <summary>
    /// Every Request that is happening inside the Simulation must be authenticated.
    /// This abstraction will ensure that a Token is passed.
    /// </summary>
    public abstract class SimulationRequest
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public int SimulationId { get; set; }
    }
}
