using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Hubs
{
    public class ResolutionHub : Hub<ITypedResolutionHub>
    {
        public Task SubscribeToResolution(string id)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, id);
        }
    }
}
