using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Schema.Response.Simulation
{
    public class SimulationUserItem
    {
        public int SimulationUserId { get; set; }

        public string DisplayName { get; set; }

        public int RoleId { get; set; }

        public SimulationUserItem(Models.Simulation.SimulationUser user)
        {
            this.SimulationUserId = user.SimulationUserId;
            this.DisplayName = user.DisplayName;
            this.RoleId = user.Role?.SimulationRoleId ?? -2;
        }

        public static implicit operator SimulationUserItem(Models.Simulation.SimulationUser user)
        {
            return new SimulationUserItem(user);
        }
    }
}
