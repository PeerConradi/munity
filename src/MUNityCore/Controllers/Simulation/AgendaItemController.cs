using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MUNity.Schema.Simulation;
using MUNityCore.Extensions;
using MUNityCore.Extensions.CastExtensions;
using MUNityCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Controllers.Simulation
{
    /// <summary>
    /// Controller to handle request to the agenda of a simulation.
    /// </summary>
    [Route("api/Simulation/[controller]")]
    [ApiController]
    public class AgendaItemController : ControllerBase, ISimulationController
    {
        public IHubContext<Hubs.SimulationHub, MUNity.Hubs.ITypedSimulationHub> HubContext { get; set; }

        private readonly SimulationService _simulationService;

        public AgendaItemController(IHubContext<Hubs.SimulationHub, MUNity.Hubs.ITypedSimulationHub> hubContext,
            SimulationService simulationService)
        {
            this.HubContext = hubContext;
            this._simulationService = simulationService;
        }

        /// <summary>
        /// Creates a new agenda item with an empty set of petitions.
        /// You need to either be the owner of the simulation or have a role that is of type Chairman.
        /// </summary>
        /// <param name="agendaItem"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> CreateAgendaItem([FromBody] CreateAgendaItemDto agendaItem)
        {
            var isAllowed = await this._simulationService.IsTokenValidAndUserChair(agendaItem);
            if (!isAllowed) isAllowed = await this._simulationService.IsTokenValidAndUserAdmin(agendaItem);
            if (!isAllowed) return BadRequest();

            var item = await this._simulationService.CreateAgendaItem(agendaItem);
            if (item == null) return BadRequest();

            _ = this.SocketGroup(agendaItem).AgendaItemAdded(item.ToAgendaItemDto());

            return Ok();
        }

        /// <summary>
        /// Returns a list of agenda items. Toggle between the options of also loading the petitions.
        /// </summary>
        /// <param name="simsimtoken"></param>
        /// <param name="simulationId"></param>
        /// <param name="withPetitions">Should the result contain the petitions</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<AgendaItemDto>>> AgendaItems([FromHeader] string simsimtoken, int simulationId, bool withPetitions = false)
        {
            var isAllowed = await this._simulationService.IsTokenValid(simulationId, simsimtoken);
            if (!isAllowed) return BadRequest();
            if (withPetitions)
            {
                var dto = await this._simulationService.GetAgendaItemsAndPetitionsDto(simulationId);
                return Ok(dto);
            }
            else
            {
                var agendaItems = await this._simulationService.GetAgendaItems(simulationId);
                return Ok(agendaItems);
            }
        }

        /// <summary>
        /// Removes the agenda item and all petitions that are on this agenda item.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> DeleteAgendaItem([FromBody]AgendaItemRequest body)
        {
            var isAllowed = await this._simulationService.IsTokenValidAndUserChairOrOwner(body);
            if (!isAllowed) return BadRequest();

            bool success = this._simulationService.RemoveAgendaItem(body.AgendaItemId);

            _ = this.SocketGroup(body)?.AgendaItemRemoved(body.AgendaItemId).ConfigureAwait(false);

            return Ok();
        }
    }
}
