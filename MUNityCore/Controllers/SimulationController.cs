using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using MUNityCore.Util.Extensions;
using MUNityCore.Models.Organization;
using MUNityCore.Models.Simulation;
using MUNityCore.Schema.Request.Simulation;
using MUNityCore.Schema.Response.Simulation;
using MUNityCore.Services;

namespace MUNityCore.Controllers
{

    /// <summary>
    /// The Controller for Simulations (online committees) etc.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SimulationController : ControllerBase
    {

        private readonly IHubContext<Hubs.SimulationHub, Hubs.ITypedSimulationHub> _hubContext;

        private readonly SimulationService _simulationService;

        public SimulationController(IHubContext<Hubs.SimulationHub, Hubs.ITypedSimulationHub> hubContext,
            SimulationService simulationService)
        {
            this._hubContext = hubContext;
            this._simulationService = simulationService;
        }

        [HttpGet]
        [Route("[action]")]
        public ActionResult<IEnumerable<SimulationResponses.SimulationList>> GetListOfSimulations()
        {
            return Ok(this._simulationService.GetSimulationFront());
        }

        [HttpPost]
        [Route("[action]")]
        public ActionResult<Simulation> CreateSimulation([FromBody]SimulationRequests.CreateSimulation request)
        {
            var result = this._simulationService.CreateSimulation(request.Name, request.Password);
            
            return Ok(result);
        }
    }
}