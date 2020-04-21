using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Schema.Request.Simulation
{
    public class DeleteRequestRequestBody
    {
        public string RequestId { get; set; }

        public string SimulationId { get; set; }

        public string HiddenToken { get; set; }
    }
}
