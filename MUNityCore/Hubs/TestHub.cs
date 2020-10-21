using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Hubs
{
    public class TestHub : Hub<ITypedTestHub>
    {

        public Task Subscribe()
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, "test");
        }
        
    }
}
