using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Schema.Response.Simulation
{
    public class SimulationRoleResponse
    {
        public int SimulationRoleId { get; set; }

        public string Name { get; set; }

        Models.Simulation.SimulationRole.RoleTypes RoleType { get; set; }

        public string Iso { get; set; }

        public SimulationRoleResponse(Models.Simulation.SimulationRole role)
        {
            SimulationRoleId = role.SimulationRoleId;
            Name = role.Name;
            role.RoleType = role.RoleType;
            role.Iso = role.Iso;
        }

        public static implicit operator SimulationRoleResponse (Models.Simulation.SimulationRole source)
        {
            return new SimulationRoleResponse(source);
        }
    }
}
