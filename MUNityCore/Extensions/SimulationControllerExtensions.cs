using MUNity.Schema.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Extensions
{
    public static class SimulationControllerExtensions
    {
        internal static MUNity.Hubs.ITypedSimulationHub SocketGroup(this Controllers.Simulation.ISimulationController controller, SimulationRequest request)
        {
            return controller.HubContext.Clients.Group($"sim_{request.SimulationId}");
        }

        internal static MUNity.Hubs.ITypedSimulationHub SocketGroup(this Controllers.Simulation.ISimulationController controller, int simulationId)
        {
            return controller.HubContext.Clients.Group($"sim_{simulationId}");
        }
    }
}
