using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MUNity.Schema.Simulation
{
    /// <summary>
    /// The body of a join request.
    /// </summary>
    public class JoinAuthenticate
    {
        [Required]
        public int SimulationId { get; set; }

        [Required]
        [MaxLength(100)]
        public string UserId { get; set; }

        /// <summary>
        /// The password of the simulation you want to enter.
        /// </summary>
        [MaxLength(100)]
        public string Password { get; set; }

        /// <summary>
        /// The display name you want to enter with.
        /// </summary>
        [MaxLength(50)]
        public string DisplayName { get; set; }
    }
}
