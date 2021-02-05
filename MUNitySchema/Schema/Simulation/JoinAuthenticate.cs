using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MUNity.Schema.Simulation
{
    /// <summary>
    /// The body of a request to join into a simulation with a given PublicId and password
    /// </summary>
    public class JoinAuthenticate
    {
        /// <summary>
        /// The id of the Simulation you want to join
        /// </summary>
        [Required]
        public int SimulationId { get; set; }

        /// <summary>
        /// The public user id of the slot you want to join in
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string UserId { get; set; }

        /// <summary>
        /// The password of the simulation you want to enter.
        /// </summary>
        [MaxLength(100)]
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// The display name you want to enter with.
        /// </summary>
        [MaxLength(50)]
        [Required]
        public string DisplayName { get; set; }
    }
}
