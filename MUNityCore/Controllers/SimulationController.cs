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
using Microsoft.AspNetCore.Authorization;

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

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<SimSimResponse>> GetSimulation([FromHeader]string simsimtoken, int id)
        {
            var simulation = await this._simulationService.GetSimulation(id);
            if (simulation == null) return NotFound();
            if (!simulation.Users.Any(n => n.Token == simsimtoken)) return Forbid();
            return (SimSimResponse)simulation;
        }

        [HttpPost]
        [Route("[action]")]
        [AllowAnonymous]
        public ActionResult<SimSimTokenResponse> CreateSimulation([FromBody]SimulationRequests.CreateSimulation request)
        {
            var result = this._simulationService.CreateSimulation(request.Name, request.Password, request.UserDisplayName);
            var adminToken = result.Roles.First().RoleKey;
            var response = new SimSimTokenResponse()
            {
                Name = result.Name,
                Token = adminToken,
                SimulationId = result.SimulationId
            };

            return Ok(response);
        }
    }
}