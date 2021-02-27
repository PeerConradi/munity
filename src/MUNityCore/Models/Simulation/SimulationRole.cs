using MUNity.Schema.Simulation;
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
        [MaxLength(5)]
        public string Iso { get; set; }

        public Simulation Simulation { get; set; }

        public SimulationRole()
        {

        }

        public SimulationRole(string iso, string name, RoleTypes roleType)
        {
            this.Iso = iso;
            this.Name = name;
            this.RoleType = roleType;
        }

        internal MUNity.Schema.Simulation.SimulationRoleDto ToSimulationRoleItem()
        {
            var item = new MUNity.Schema.Simulation.SimulationRoleDto()
            {
                Iso = Iso,
                Name = Name,
                RoleType = RoleType,
                SimulationRoleId = SimulationRoleId
            };
            return item;
        }
    }
}
