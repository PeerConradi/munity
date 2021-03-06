using Microsoft.AspNetCore.SignalR;
using MUNityCore.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityCore.Extensions.CastExtensions;

namespace MUNityCore.Hubs
{
    public class SimulationHub : Hub<MUNity.Hubs.ITypedSimulationHub>
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

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            ConnectionUsers.ConnectionIds.Remove(Context.ConnectionId);
            if (this.Context?.ConnectionId != null && this._service != null)
            {
                await this._service.RemoveConnectionKey(this.Context.ConnectionId);
            }
            await base.OnDisconnectedAsync(exception);
        }

        public override Task OnConnectedAsync()
        {
            ConnectionUsers.ConnectionIds.Add(Context.ConnectionId);
            return base.OnConnectedAsync();
        }

    }

    public static class ConnectionUsers
    {
        public static List<string> ConnectionIds { get; set; } = new List<string>();
    }
}
