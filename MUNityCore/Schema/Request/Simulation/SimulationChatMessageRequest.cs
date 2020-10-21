using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Schema.Request.Simulation
{
    public class SimulationChatMessageRequest
    {
        public string SimulationId { get; set; }

        public string UserToken { get; set; }

        public string Text { get; set; }
    }
}
