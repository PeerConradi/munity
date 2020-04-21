using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Schema.Request.Simulation
{
    public class NewRequestRequestBody
    {
        public string SimulationId { get; set; }

        public string HiddenToken { get; set; }

        public string Token { get; set; }

        public string Type { get; set; }

        public string Message { get; set; }
    }
}
