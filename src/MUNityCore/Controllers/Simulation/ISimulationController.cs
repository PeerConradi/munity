using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Controllers.Simulation
{
    public interface ISimulationController
    {
        IHubContext<Hubs.SimulationHub, MUNity.Hubs.ITypedSimulationHub> HubContext { get; set; }
    }
}
