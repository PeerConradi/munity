using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Schema.Response.Simulation
{
    public class SimulationAuthSchema
    {
        public int SimulationUserId { get; set; }

        public bool CanCreateRole { get; set; }

        public bool CanSelectRole { get; set; }

        public bool CanEditResolution { get; set; }

        public bool CanEditListOfSpeakers { get; set; }

        public SimulationAuthSchema(Models.Simulation.SimulationUser user)
        {
            this.SimulationUserId = user.SimulationUserId;
            this.CanCreateRole = user.CanCreateRole;
            this.CanSelectRole = user.CanSelectRole;
            this.CanEditResolution = user.CanEditResolution;
            this.CanEditListOfSpeakers = user.CanEditListOfSpeakers;
        }

        public static implicit operator SimulationAuthSchema(Models.Simulation.SimulationUser user)
        {
            return new SimulationAuthSchema(user);
        }
    }
}
