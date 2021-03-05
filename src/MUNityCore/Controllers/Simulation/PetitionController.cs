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

        /// <summary>
        /// Gives back a list of simulation Petition type templates you can use with
        /// ApplyPetitionPreset <see cref="ApplyPetitionPreset"/>
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public ActionResult<List<string>> PetitionTemplateNames()
        {
            var list = new List<string>();
            string path = AppContext.BaseDirectory + "assets/templates/petitions/";
            if (!System.IO.Directory.Exists(path))
                return NotFound("Directory for presets not found: " + path);

            var dir = new System.IO.DirectoryInfo(path);
            var files = dir.GetFiles("*.csv");
            if (!files.Any())
            {
                return NotFound("No files in directory: " + path);
            }
            return Ok(files.Select(n => n.Name.Substring(0, n.Name.Length - 4)).ToList());
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<PetitionTypeDto>> CreatePetitionType([FromBody]CreatePetitionTypeRequest body)
        {
            var isAllowed = await _simulationService.IsTokenValidAndUserChairOrOwner(body);
            if (!isAllowed) return BadRequest();

            Models.Simulation.PetitionType type = _simulationService.CreatePetitionType(body);
            if (type == null)
                return NotFound("Something went wrong");

            return Ok(type.ToPetitionTypeDto());
        }

        /// <summary>
        /// Not implemented yet!
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<ActionResult> AddPetitionTypeToSimulation([FromBody] AddPetitionTypeRequestBody body)
        {
            var isAllowed = await this._simulationService.IsTokenValidAndUserAdmin(body);
            if (!isAllowed) return BadRequest();

            var success = await this._simulationService.AddPetitionTypeToSimulation(body);
            if (!success)
                return NotFound("Something went wrong maybe the simulation or petitiontype was not found!");

            return Ok();
        }

        /// <summary>
        /// Returns a list of all possible petition types.
        /// This petition types can be linked to a conference to use them there.
        /// </summary>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult<List<Models.Simulation.PetitionType>>> AllPetitionTypes()
        {
            var context = _simulationService.GetDatabaseInstance();
            var petitionTypes = context.PetitionTypes.Select(n => n.ToPetitionTypeDto()).ToList();
            return Ok(petitionTypes);
        }


        /// <summary>
        /// Creates a new Petition inside an Agenda Item of a conference.
        /// </summary>
        /// <param name="petition"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<ActionResult> MakePetition([FromBody] CreatePetitionRequest petition)
        {
            try
            {
                var user = this._simulationService.GetSimulationUser(petition.SimulationId, petition.Token);
                if (user == null) return BadRequest();
                petition.PetitionUserId = user.SimulationUserId;
                var createdPetition = this._simulationService.SubmitPetition(petition);
                if (createdPetition != null)
                {
                    await this.SocketGroup(petition).PetitionAdded(createdPetition.ToPetitionDto());
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return Problem("Unable to create the Petition. " + ex.Message);
            }
            
            return Problem("Unable to create the Petition.");
        }



        //[HttpPut]
        //[Route("[action]")]
        //[AllowAnonymous]
        //public async Task<ActionResult> AcceptPetition([FromBody] PetitionDto petition)
        //{
        //    var user = this._simulationService.GetSimulationUser(petition.SimulationId, petition.Token);
        //    if (!user.CanCreateRole) return BadRequest();
        //    await this._hubContext.Clients.Group($"sim_{petition.SimulationId}").UserPetitionAccepted(petition);
        //    return Ok();
        //}

        /// <summary>
        /// Removes a petition from an agenda item of a conference.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<ActionResult> DeletePetition([FromBody] PetitionInteractRequest body)
        {
            if (body == null || string.IsNullOrEmpty(body.PetitionId))
                return BadRequest();

            var isValid = await this._simulationService.IsPetitionInteractionAllowed(body);
            if (!isValid) return BadRequest();

            var removed = this._simulationService.RemovePetition(body);
            if (removed)
            {
                var mdl = new PetitionInteractedDto()
                {
                    AgendaItemId = body.AgendaItemId,
                    PetitionId = body.PetitionId
                };
                _ = this.SocketGroup(body).PetitionDeleted(mdl).ConfigureAwait(false);
                return Ok();
            }
            else
            {
                return NotFound("Petition was not removed because it wasnt found");
            }
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<ActionResult> ApplyPetitionPreset([FromBody] ApplyPetitionTemplate body)
        {
            var isChair = await this._simulationService.IsTokenValidAndUserChair(body);
            var isAdmin = await this._simulationService.IsTokenValidAndUserAdmin(body);
            if (!isChair && !isAdmin) return BadRequest();

            var path = AppContext.BaseDirectory + "assets/templates/petitions/" + body.Name + ".csv";
            if (!System.IO.File.Exists(path)) return NotFound("Templatefile not found!");

            var template = this._simulationService.LoadSimulationPetitionTemplate(path, body.Name);
            if (template == null || !template.Entries.Any()) return Problem("Unable to load the template or it has no entries");

            this._simulationService.ApplyPetitionTemplateToSimulation(template, body.SimulationId);
            return Ok();
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<PetitionTypeSimulationDto>>> SimulationPetitionTypes([FromHeader] string simsimtoken, int simulationId)
        {
            var isallowed = await this._simulationService.IsTokenValid(simulationId, simsimtoken);
            if (!isallowed) return BadRequest();
            var types = this._simulationService.GetPetitionTypesOfSimulation(simulationId);
            var list = types.Select(n => n.ToDto()).ToList();
            return Ok(list);
        }
    }
}
