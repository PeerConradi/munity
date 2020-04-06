using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Schema.Request.Simulation
{
    public interface ISimulationChatMessageRequest
    {
        string SimulationId { get; set; }

        string UserToken { get; set; }

        string Text { get; set; }
    }
}
