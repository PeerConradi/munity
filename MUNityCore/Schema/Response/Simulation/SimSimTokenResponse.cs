using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Schema.Response.Simulation
{
    public class SimSimTokenResponse
    {
        public int SimulationId { get; set; }

        public string Name { get; set; }

        public string Token { get; set; }

        public string Pin { get; set; }

        public SimSimTokenResponse()
        {

        }

        public SimSimTokenResponse(Models.Simulation.SimulationUser user)
        {
            SimulationId = user.Simulation.SimulationId;
            Name = user.Simulation.Name;
            Token = user.Token;
            Pin = user.Pin;
        }

        public static implicit operator SimSimTokenResponse (Models.Simulation.SimulationUser user)
        {
            return new SimSimTokenResponse(user);
        }
    }
}
