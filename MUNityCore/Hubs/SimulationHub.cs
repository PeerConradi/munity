﻿using Microsoft.AspNetCore.SignalR;
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

        //public override Task OnDisconnectedAsync(Exception exception)
        //{
        //    // This is working great in theory
        //    // But has a bug: When the user reloads the page the connection will be lost for a short time
        //    // There needs to be a timeout function to handle this!
        //    //var game = this._service.GetGameWithConnectionInside(this.Context.ConnectionId);
        //    //if (game != null)
        //    //{
        //    //    var user = game.SignalRConnections[this.Context.ConnectionId];
        //    //    game.RemoveUser(user);
        //    //    this.Clients.Group(game.SimulationId.ToString()).UserLeft(user);
        //    //}
        //    return base.OnDisconnectedAsync(exception);
        //}

        //public void Join(string gameid, string token)
        //{
        //    //Console.WriteLine("Test");
        //    //var game = _service.GetSimulation(gameid.ToIntOrDefault());
        //    //if (game != null)
        //    //{
        //    //    if (game.HiddenTokenValid(token))
        //    //    {
        //    //        var user = game.GetUserByHiddenToken(token);
        //    //        if (user != null && !game.SignalRConnections.ContainsKey(this.Context.ConnectionId))
        //    //        {
        //    //            game.SignalRConnections.Add(this.Context.ConnectionId, user);
        //    //        }
        //    //        try
        //    //        {
        //    //            this.Groups.AddToGroupAsync(this.Context.ConnectionId, gameid);
        //    //        }
        //    //        catch (Exception)
        //    //        {
        //    //            throw;
        //    //        }
                    
        //    //    }
        //    //}
        //}
    }
}