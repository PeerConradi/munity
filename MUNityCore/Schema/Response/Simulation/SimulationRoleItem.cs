using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Schema.Response.Simulation
{
    public class SimulationRoleItem
    {
        public int SimulationRoleId { get; set; }

        public string Name { get; set; }

        public Models.Simulation.SimulationRole.RoleTypes RoleType { get; set; }

        public string Iso { get; set; }

        public IEnumerable<string> Users { get; set; }

        public SimulationRoleItem(Models.Simulation.SimulationRole role, IEnumerable<Models.Simulation.SimulationUser> usersWithRole)
        {
            this.SimulationRoleId = role.SimulationRoleId;
            this.Name = role.Name;
            this.RoleType = role.RoleType;
            this.Iso = role.Iso;
            this.Users = usersWithRole.Select(n => n.DisplayName);
        }
    }
}
