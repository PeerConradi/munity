using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Simulation
{
    public class UserRoleChangedEventArgs : EventArgs
    {
        public int SimulationId { get; private set; }

        public int UserId { get; private set; }

        public int RoleId { get; private set; }

        public UserRoleChangedEventArgs(int simulationId, int userId, int roleId)
        {
            this.SimulationId = simulationId;
            this.UserId = userId;
            this.RoleId = roleId;
        }
    }
}
