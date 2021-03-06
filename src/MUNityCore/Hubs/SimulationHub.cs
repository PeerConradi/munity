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
            bool outVal = true;
            ConnectionUsers.ConnectionIds.TryRemove(Context.ConnectionId, out outVal);
            if (this.Context?.ConnectionId != null && this._service != null)
            {
                await this._service.RemoveConnectionKey(this.Context.ConnectionId);
            }
            await base.OnDisconnectedAsync(exception);
        }

        public override Task OnConnectedAsync()
        {
            ConnectionUsers.ConnectionIds.TryAdd(Context.ConnectionId, true);
            return base.OnConnectedAsync();
        }

    }

    public static class ConnectionUsers
    {

        public static System.Collections.Concurrent.ConcurrentDictionary<string, bool> ConnectionIds { get; set; }
    }
}
