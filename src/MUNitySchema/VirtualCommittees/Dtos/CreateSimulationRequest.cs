using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MUNity.VirtualCommittees.Dtos
{
    /// <summary>
    /// A request body to create a new simulation/virual Committee.
    /// </summary>
    public class CreateSimulationRequest
    {
        /// <summary>
        /// The display name of the simulation.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The password for administration. This password will be asked if someone who is not the owner
        /// of the simulation will need to enter to change the settings.
        /// </summary>
        [Required]
        public string AdminPassword { get; set; }

        /// <summary>
        /// The display name you want to have because the one creating the simulation will also be the
        /// first user to join and be added to the admin list.
        /// </summary>
        [Required]
        public string UserDisplayName { get; set; }
    }
}
