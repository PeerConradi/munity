using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUNityCore.Models.Simulation
{
    public class SimulationRole
    {

        public enum RoleTypes
        {
            Spectator,
            Chairman,
            Delegate,
            Moderator
        }

        /// <summary>
        /// The Id for this SimulationRole.
        /// </summary>
        public int SimulationRoleId { get; set; }

        /// <summary>
        /// The name of the Role for example Delegate of Germany.
        /// </summary>
        [MaxLength(250)]
        public string Name { get; set; }

        public RoleTypes RoleType { get; set; }

        /// <summary>
        /// The iso of this role. For example DE for Germany or UK for United Kingdom.
        /// Read all about the Alpha-2 codes here:
        /// https://en.wikipedia.org/wiki/List_of_ISO_3166_country_codes
        /// </summary>
        [MaxLength(2)]
        public string Iso { get; set; }

        /// <summary>
        /// The RoleKey is used to give to the person so they can
        /// register into this specific role.
        /// </summary>
        [MaxLength(32)]
        public string RoleKey { get; set; }

        /// <summary>
        /// How many users can max. go into this role.
        /// </summary>
        public int RoleMaxSlots { get; set; }

        public Simulation Simulation { get; set; }
    }
}
