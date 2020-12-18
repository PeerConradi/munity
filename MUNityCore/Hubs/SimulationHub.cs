using Microsoft.AspNetCore.SignalR;
using MUNityCore.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Hubs
{
    public class SimulationHub : Hub<ITypedSimulationHub>
    {
        private readonly Services.SimulationService _service;
        public SimulationHub(Services.SimulationService service)
        {
            _service = service;
        }

        public void Test()
        {
            Console.WriteLine("This works just fine!");
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {

            var simulation = this._service.GetSimulationAndUserByConnectionId(this.Context.ConnectionId);
            var disconnectedUser = simulation.Users.FirstOrDefault(n => n.HubConnections.Any(a => a.ConnectionId == this.Context.ConnectionId));
            disconnectedUser.HubConnections.RemoveAll(n => n.ConnectionId == this.Context.ConnectionId);
            if (!disconnectedUser.HubConnections.Any())
            {
                this.Clients.Group($"sim_{simulation.SimulationId}").UserDisconnected(simulation.SimulationId, disconnectedUser);
            }
            return base.OnDisconnectedAsync(exception);
        }

    }
}
