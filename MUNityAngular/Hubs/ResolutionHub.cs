using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Hubs
{
    public class ResolutionHub : Hub
    {
        public Task SubscribeToGroup()
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, "test");
        }

    }
}
