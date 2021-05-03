using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MUNity.Hubs;
using MUNity.Schema.Simulation;
using MUNity.Schema.Simulation.Voting;
using MUNityCore.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Controllers.Simulation
{
    /// <summary>
    /// Controller to handle votings inside a simulation.
    /// </summary>
    [Route("api/Sim/Voting")]
    [ApiController]
    public class SimulationVoteController : ControllerBase
    {

        public IHubContext<SimulationHub, ITypedSimulationHub> HubContext { get; set; }

        private Services.SimulationService _simulationService;

        public SimulationVoteController(IHubContext<SimulationHub, ITypedSimulationHub> hubContext, Services.SimulationService simulationService)
        {
            this.HubContext = hubContext;
            this._simulationService = simulationService;
        }

        /// <summary>
        /// Creates a new voting inside the simulation
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Create([FromBody] CreateSimulationVoting body)
        {
            var isAllowed = await this._simulationService.IsTokenValidAndUserChairOrOwner(body);
            if (!isAllowed)
                return BadRequest();

            Models.Simulation.SimulationVoting voting = this._simulationService.CreateVoting(body);
            if (voting == null)
                return BadRequest();

            SimulationVotingDto dto = this._simulationService.GetCurrentVoting(body.SimulationId);
            GetSimulationHub(body)?.VoteCreated(dto);

            return Ok();
        }

        /// <summary>
        /// Returns information about the currently ongoing voting
        /// </summary>
        /// <param name="simsimtoken"></param>
        /// <param name="simulationId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<CreatedVoteModel>> GetActiveVote([FromHeader]string simsimtoken, int simulationId)
        {
            var isAllowed = await this._simulationService.IsTokenValidAsync(simulationId, simsimtoken);
            if (!isAllowed)
                return BadRequest();

            SimulationVotingDto voting = this._simulationService.GetCurrentVoting(simulationId);
            if (voting == null)
                return NotFound();

            return Ok(voting);
        }

        /// <summary>
        /// votes for a choice inside an ongoing voting.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> Vote(UserVoteRequest body)
        {
            var allowed = await this._simulationService.IsTokenValidAndUserDelegate(body);
            if (!allowed)
                return BadRequest();

            bool success = this._simulationService.Vote(body);
            if (!success)
                return NotFound();

            var userId = this._simulationService.GetSimulationUserId(body.SimulationId, body.Token);

            if (userId.HasValue)
                GetSimulationHub(body)?.Voted(new VotedEventArgs(body.VotingId, userId.Value, body.Choice));

            return Ok();
        }

        private ITypedSimulationHub GetSimulationHub(SimulationRequest request)
        {
            return this.HubContext?.Clients?.Group($"sim_{request.SimulationId}");
        }
    }
}
