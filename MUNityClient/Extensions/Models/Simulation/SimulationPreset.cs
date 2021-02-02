using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityClient.Models.Simulation
{

    [Obsolete("Use the MUNityBase /MUNitySchema Package!")]
    public class SimulationPreset
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<MUNity.Schema.Simulation.SimulationRoleItem> Roles { get; set; }
    }
}
