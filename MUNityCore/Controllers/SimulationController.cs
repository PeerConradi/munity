using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MUNityCore.Util.Extenstions;
using MUNityCore.DataHandlers.EntityFramework.Models;
using MUNityCore.Models.Organisation;
using MUNityCore.Models.Simulation;
using MUNityCore.Schema.Request.Simulation;
using MUNityCore.Schema.Response.Simulation;
using MUNityCore.Services;

namespace MUNityCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimulationController : ControllerBase
    {

        IHubContext<Hubs.SimulationHub, Hubs.ITypedSimulationHub> _hubContext;

        public SimulationController(IHubContext<Hubs.SimulationHub, Hubs.ITypedSimulationHub> hubContext)
        {
            this._hubContext = hubContext;
        }

        

    }
}