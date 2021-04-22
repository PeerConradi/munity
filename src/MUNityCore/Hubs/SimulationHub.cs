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

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            
            ConnectionUsers.ConnectedUser outVal = null;
            ConnectionUsers.ConnectionIds.TryRemove(Context.ConnectionId, out outVal);
            Console.WriteLine("Simulation User Disconnected: " + outVal.SimulationUserId.ToString());
            await NotifyUsersChanged(outVal.SimulationId);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SignIn(int simulationId, int userId)
        {
            var newConnection = new ConnectionUsers.ConnectedUser()
            {
                SimulationId = simulationId,
                SimulationUserId = userId
            };
            Console.WriteLine("Simulation User Connected: " + userId.ToString());
            ConnectionUsers.ConnectionIds.TryAdd(Context.ConnectionId, newConnection);
            await NotifyUsersChanged(simulationId);
            
        }

        private Task NotifyUsersChanged(int simulationId)
        {
            return Clients.Group($"sim_{simulationId}")
                .ConnectedUsersChanged(ConnectionUsers.ConnectionIds.Where(n =>
                n.Value.SimulationId == simulationId)
                .Select(n => n.Value.SimulationUserId).ToList());
        }

        //public override Task OnConnectedAsync()
        //{
        //    if (ConnectionUsers.ConnectionIds == null)
        //        ConnectionUsers.ConnectionIds = new System.Collections.Concurrent.ConcurrentDictionary<string, bool>();
        //    ConnectionUsers.ConnectionIds.TryAdd(Context.ConnectionId, true);
        //    return base.OnConnectedAsync();
        //}

    }

    public static class ConnectionUsers
    {

        public static System.Collections.Concurrent.ConcurrentDictionary<string, ConnectedUser> ConnectionIds { get; set; } = new System.Collections.Concurrent.ConcurrentDictionary<string, ConnectedUser>();

        public class ConnectedUser
        {
            public int SimulationId { get; set; }

            public int SimulationUserId { get; set; }
        }
    }
}
