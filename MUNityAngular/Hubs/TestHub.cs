using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Hubs
{
    public class TestHub : Hub<ITypedTestHub>
    {

        public void FromClient(string msg)
        {
            Console.WriteLine("Got something from: " + Context.ConnectionId + ": " + msg);
        }

        public Task Subscribe()
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, "test");
        }
        
    }
}
