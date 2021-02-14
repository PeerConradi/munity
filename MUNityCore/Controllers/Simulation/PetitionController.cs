using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MUNity.Schema.Simulation;
using MUNity.Schema.Simulation.Managment;
using MUNityCore.Extensions;
using MUNityCore.Extensions.CastExtensions;
using MUNityCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Controllers.Simulation
{
    [Route("api/Simulation/[controller]")]
    [ApiController]
    public class PetitionController : ControllerBase, ISimulationController
    {
        public IHubContext<Hubs.SimulationHub, MUNity.Hubs.ITypedSimulationHub> HubContext { get; set; }

        private readonly SimulationService _simulationService;

        public PetitionController(IHubContext<Hubs.SimulationHub, MUNity.Hubs.ITypedSimulationHub> hubContext,
            SimulationService simulationService)
        {
            this.HubContext = hubContext;
            this._simulationService = simulationService;
        }

        [HttpGet]
        [Route("[action]")]
        public ActionResult<List<string>> PetitionTemplateNames()
        {
            var list = new List<string>();
            list.Add("DMUN2");
            return Ok(list);
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<ActionResult> AddPetitionTypeToSimulation([FromBody] AddPetitionTypeRequestBody body)
        {
            var isAllowed = await this._simulationService.IsTokenValidAndUserAdmin(body);
            if (!isAllowed) return Forbid();
            return Ok();
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult<List<Models.Simulation.PetitionType>>> AllPetitionTypes()
        {
            var context = _simulationService.GetDatabaseInstance();
            var petitionTypes = await context.PetitionTypes.ToListAsync();
            return Ok(petitionTypes);
        }



        [HttpPut]
        [Route("[action]")]
        public async Task<ActionResult> MakePetition([FromBody] CreatePetitionRequest petition)
        {
            var user = this._simulationService.GetSimulationUser(petition.SimulationId, petition.Token);
            if (user == null) return Forbid();
            petition.PetitionUserId = user.SimulationUserId;
            var createdPetition = this._simulationService.SubmitPetition(petition);
            if (createdPetition != null)
            {
                await this.SocketGroup(petition).UserPetition(createdPetition.ToPetitionDto());
                return Ok();
            }
            return Problem("Unable to create the Petition.");
        }



        //[HttpPut]
        //[Route("[action]")]
        //[AllowAnonymous]
        //public async Task<ActionResult> AcceptPetition([FromBody] PetitionDto petition)
        //{
        //    var user = this._simulationService.GetSimulationUser(petition.SimulationId, petition.Token);
        //    if (!user.CanCreateRole) return Forbid();
        //    await this._hubContext.Clients.Group($"sim_{petition.SimulationId}").UserPetitionAccepted(petition);
        //    return Ok();
        //}

        //[HttpPut]
        //[Route("[action]")]
        //[AllowAnonymous]
        //public async Task<ActionResult> DeletePetition([FromBody] PetitionDto petition)
        //{
        //    var user = this._simulationService.GetSimulationUser(petition.SimulationId, petition.Token);
        //    if (petition.PetitionUserId != user.SimulationUserId && !user.CanCreateRole) return Forbid();
        //    await this._hubContext.Clients.Group($"sim_{petition.SimulationId}").UserPetitionDeleted(petition);
        //    return Ok();
        //}

        [HttpPut]
        [Route("[action]")]
        public async Task<ActionResult> ApplyPetitionPreset([FromBody] ApplyPetitionTemplate body)
        {
            var isChair = await this._simulationService.IsTokenValidAndUserChair(body);
            var isAdmin = await this._simulationService.IsTokenValidAndUserAdmin(body);
            if (!isChair && !isAdmin) return Forbid();

            var path = AppContext.BaseDirectory + "assets\\templates\\petitions\\" + body.Name + ".csv";
            if (!System.IO.File.Exists(path)) return NotFound("Templatefile not found!");

            var template = this._simulationService.LoadSimulationPetitionTemplate(path, "DMUN");
            if (template == null || !template.Entries.Any()) return Problem("Unable to load the template or it has no entries");

            this._simulationService.ApplyPetitionTemplateToSimulation(template, body.SimulationId);
            return Ok();
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<PetitionTypeSimulationDto>>> SimulationPetitionTypes([FromHeader] string simsimtoken, int simulationId)
        {
            var isallowed = await this._simulationService.IsTokenValid(simulationId, simsimtoken);
            if (!isallowed) return Forbid();
            var types = this._simulationService.GetPetitionTypesOfSimulation(simulationId);
            var list = types.Select(n => n.ToDto());
            return Ok(list);
        }
    }
}
