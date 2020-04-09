using Microsoft.AspNetCore.SignalR;
using MUNityAngular.Util.Extenstions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Hubs
{
    public class SimulationHub : Hub<ITypedSimulationHub>
    {
        Services.SimSimService _service;
        public SimulationHub(Services.SimSimService service)
        {
            _service = service;
        }

        public void Test()
        {
            Console.WriteLine("This works just fine!");
        }

        public void Join(string gameid, string token)
        {
            //Console.WriteLine("Test");
            var game = _service.GetSimSim(gameid.ToIntOrDefault());
            if (game != null)
            {
                if (game.HiddenTokenValid(token))
                {
                    this.Groups.AddToGroupAsync(this.Context.ConnectionId, gameid);
                }
            }
        }


    }
}
