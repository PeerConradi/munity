using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Schema.Request.Simulation
{
    public class SetRoleRequestBody
    {
        public string SimulationId { get; set; }

        public string Token { get; set; }

        public string Role { get; set; }
    }
}
