using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MUNityCore.Models.Simulation;
using MUNityCore.Services;
using Microsoft.AspNetCore.Authorization;
using MUNity.Schema.Simulation;
using MUNityCore.Extensions.CastExtensions;
using Microsoft.EntityFrameworkCore;
using MUNitySchema.Schema.Simulation.Resolution;
using MUNity.Schema.Simulation.Managment;

namespace MUNityCore.Controllers
{

    /// <summary>
    /// The Controller for Simulations (online committees) etc.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SimulationController : ControllerBase
    {

        private readonly IHubContext<Hubs.SimulationHub, MUNity.Hubs.ITypedSimulationHub> _hubContext;

        private readonly SimulationService _simulationService;

        public SimulationController(IHubContext<Hubs.SimulationHub, MUNity.Hubs.ITypedSimulationHub> hubContext,
            SimulationService simulationService)
        {
            this._hubContext = hubContext;
            this._simulationService = simulationService;
        }

        /// <summary>
        /// Checks if the controller is reachable and returns a Version of this controller.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public ActionResult<string> IsOnline()
        {
            return Ok("beta-0.0.6");
        }

        /// <summary>
        /// Returns a list of Simulations. Depricated
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<SimulationListItem>> GetListOfSimulations()
        {
            var result = this._simulationService.GetSimulations().Select(n => n);
            return Ok(result);
        }

        /// <summary>
        /// Returns the general information of a simulation containing the users and
        /// the roles.
        /// </summary>
        /// <param name="simsimtoken">a token to access this simulation. This is the token of the simulation user, not a token of a munity platform user.</param>
        /// <param name="simulationId">The id of the simulation. Should be a number between 100,000 and 999,999</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<SimulationResponse>> Simulation([FromHeader] string simsimtoken, int simulationId)
        {
            var isAllowed = await this._simulationService.IsTokenValid(simulationId, simsimtoken);
            if (!isAllowed) return Forbid();

            var response = this._simulationService.GetSimulationResponse(simulationId);
            return Ok(response);
        }

        /// <summary>
        /// Returns the id of the List of speakers of the given simulation. This will return null if tthere is no List
        /// of Speaker at this simulation.
        /// </summary>
        /// <param name="simsimtoken"></param>
        /// <param name="simulationId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> GetListOfSpeakersId([FromHeader] string simsimtoken, int simulationId)
        {
            var isValid = await this._simulationService.IsTokenValid(simulationId, simsimtoken);
            if (!isValid) return Forbid();
            return this._simulationService.GetSpeakerlistIdOfSimulation(simulationId);
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> GetResolutionId([FromHeader] string simsimtoken, int simulationId)
        {
            var isValid = await this._simulationService.IsTokenValid(simulationId, simsimtoken);
            if (!isValid) return Forbid();
            return this._simulationService.GetSpeakerlistIdOfSimulation(simulationId);
        }

        /// <summary>
        /// Initializes a new List of Speaker and will return the created list.
        /// This function will be refactored with a HttpPost method with the same name
        /// and a SimulationRequest as the body.
        /// </summary>
        /// <param name="simsimtoken"></param>
        /// <param name="simulationId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<MUNity.Models.ListOfSpeakers.ListOfSpeakers>> InitListOfSpeakers([FromHeader] string simsimtoken, int simulationId)
        {
            var isAllowed = await this._simulationService.IsTokenValidAndUserChair(simulationId, simsimtoken);
            if (!isAllowed) return Forbid();
            // TODO: Send Socket information.
            return Ok(this._simulationService.InitListOfSpeakers(simulationId));
        }


        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<SimulationUserSetup>> CreateUser([FromHeader]string simsimtoken, int id)
        {
            var isAllowed = await this._simulationService.IsTokenValidAndUserChair(id, simsimtoken);
            if (!isAllowed) return Forbid();
            var simulation = await this._simulationService.GetSimulation(id);
            if (simulation == null) return NotFound();
            var newUser = this._simulationService.CreateUser(simulation, "");
            return Ok(newUser.AsUserSetup());
        }

        [HttpGet]
        [Route("[action]")]
        public ActionResult<List<string>> PetitionTemplateNames()
        {
            var list = new List<string>();
            list.Add("DMUN2");
            return Ok(list);
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public ActionResult<SimulationUserSetup> GetUsersAsAdmin([FromHeader]string simsimtoken, int id)
        {
            var user = this._simulationService.GetSimulationUser(id, simsimtoken);
            if (user == null || user.CanCreateRole == false) return Forbid();
            var users = this._simulationService.GetSimulationUsers(id);
            users.Include(n => n.Role)
                .Include(n => n.HubConnections)
                .Include(n => n.Simulation).ToList();
            var result = users.Select(n => n.AsUserSetup()).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<bool>> IsUserOnline([FromHeader]string simsimtoken, int simulationId, int userId)
        {
            var isAllowed = await this._simulationService.IsTokenValid(simulationId, simsimtoken);
            if (!isAllowed) return Forbid();
            return Ok(this._simulationService.UserOnline(simulationId, userId));
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<SimulationUserItem>> GetUsersDefault([FromHeader]string simsimtoken, int id)
        {
            var isAllowed = await this._simulationService.IsTokenValid(id, simsimtoken);
            if (!isAllowed) return Forbid();
            var users = this._simulationService.GetSimulationUsers(id);
            users.Include(n => n.Role)
                .Include(n => n.HubConnections)
                .Include(n => n.Simulation).ToList();
            var result = users.Select(n => n.AsUserItem());
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<SimulationRoleItem>>> GetSimulationRoles([FromHeader]string simsimtoken, int id)
        {
            var isAllowed = await this._simulationService.IsTokenValid(id, simsimtoken);
            if (!isAllowed) return Forbid();
            var roles = await this._simulationService.GetSimulationRoles(id);
            var models = roles.Select(n => n.AsRoleItem());
            return Ok(models);
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public ActionResult<SimulationAuthSchema> GetSimulationAuth([FromHeader]string simsimtoken, int id)
        {
            var user = this._simulationService.GetSimulationUser(id, simsimtoken);
            if (user == null) return NotFound();
            return Ok(user.AsAuthSchema());
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult> PickRole([FromHeader]string simsimtoken, int simulationId, int roleId)
        {
            var simulation = await this._simulationService.GetSimulationWithUsersAndRoles(simulationId);
            if (simulation == null) return NotFound();
            var user = simulation.Users.FirstOrDefault(n => n.Token == simsimtoken);
            if (user == null) return NotFound();
            if (!user.CanSelectRole) return Forbid();
            var role = simulation.Roles.FirstOrDefault(n => n.SimulationRoleId == roleId);
            if (role == null) return NotFound();
            this._simulationService.BecomeRole(simulation, user, role);
            await this._hubContext.Clients.Group($"sim_{simulationId}").UserRoleChanged(new UserRoleChangedEventArgs(simulationId, user.SimulationUserId, roleId));
            return Ok();
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult> SetUserRole([FromHeader]string simsimtoken, int simulationId, int userId, int roleId)
        {
            var simulation = await this._simulationService.GetSimulationWithUsersAndRoles(simulationId);
            if (simulation == null) return NotFound();
            var user = simulation.Users.FirstOrDefault(n => n.Token == simsimtoken);
            if (user == null || user.CanCreateRole == false) return Forbid();
            var targetUser = simulation.Users.FirstOrDefault(n => n.SimulationUserId == userId);
            if (targetUser == null) return NotFound();
            if (roleId == -2)
            {
                this._simulationService.BecomeRole(simulation, targetUser, null);
                await this._hubContext.Clients.Group($"sim_{simulationId}").UserRoleChanged(new UserRoleChangedEventArgs(simulationId, user.SimulationUserId, roleId));
                return Ok();
            }
            var targetRole = simulation.Roles.FirstOrDefault(n => n.SimulationRoleId == roleId);
            if (targetRole == null) return NotFound();

            this._simulationService.BecomeRole(simulation, targetUser, targetRole);
            await this._hubContext.Clients.Group($"sim_{simulationId}").UserRoleChanged(new UserRoleChangedEventArgs(simulationId, user.SimulationUserId, roleId));
            return Ok();
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult> ApplyPreset([FromHeader]string simsimtoken, int simulationId, string presetId)
        {
            var simulation = await this._simulationService.GetSimulationWithUsersAndRoles(simulationId);
            if (simulation == null) return NotFound();
            if (!simulation.Users.Any(n => n.Token == simsimtoken && n.CanCreateRole)) return Forbid();
            var preset = this._simulationService.Presets.FirstOrDefault(n => n.Id == presetId);
            if (preset == null) return NotFound();
            this._simulationService.ApplyPreset(simulation, preset);
            await this._hubContext.Clients.Group($"sim_{simulationId}").RolesChanged(new RolesChangedEventArgs(simulationId, simulation.Roles.Select(n => n.AsRoleItem())));
            return Ok();
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult> RemoveSimulationUser([FromHeader]string simsimtoken, int simulationId, int userId)
        {
            var simulation = await this._simulationService.GetSimulationWithUsersAndRoles(simulationId);
            if (simulation == null) return NotFound();
            if (!simulation.Users.Any(n => n.Token == simsimtoken && n.CanCreateRole)) return Forbid();
            await this._simulationService.RemoveUser(simulationId, userId);
            //await this._hubContext.Clients.Group($"sim_{simulationId}").
            return Ok();
        }

        /// <summary>
        /// Get the Role Presets
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<SimulationRolesPreset>> GetPresets()
        {
            var presets = this._simulationService.Presets.Select(n => new SimulationRolesPreset()
            {
                Id = n.Id,
                Name = n.Name,
                Roles = n.Roles.Select(a => a.ToSimulationRoleItem()).ToList()
            });
            return Ok(this._simulationService.Presets);
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<ActionResult> SetPhase([FromBody]SetPhaseRequest body)
        {
            var isAllowed = await this._simulationService.IsTokenValidAndUserChair(body);
            if (!isAllowed) return Forbid();
            var hasChanged = await this._simulationService.SetPhase(body.SimulationId, body.SimulationPhase);
            if (hasChanged)
            {
                _ = this._hubContext.Clients.Group($"sim_{body.SimulationId}").PhaseChanged(body.SimulationId, body.SimulationPhase);
            }
            return Ok();
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<ActionResult> AddPetitionTypeToSimulation([FromBody] AddPetitionTypeRequestBody body)
        {
            var isAllowed = await this._simulationService.IsTokenValidAndUserAdmin(body);
            if (!isAllowed) return Forbid();
            return Ok();
        }



        [HttpPost]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult> CreateAgendaItem([FromBody]CreateAgendaItemDto agendaItem)
        {
            var isAllowed = await this._simulationService.IsTokenValidAndUserChair(agendaItem);
            if (!isAllowed) isAllowed = await this._simulationService.IsTokenValidAndUserAdmin(agendaItem);
            if (!isAllowed) return Forbid();

            var item = await this._simulationService.CreateAgendaItem(agendaItem);
            if (item == null) return Problem();
            return Ok();
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult> AgendaItems([FromHeader]string simsimtoken, int simulationId)
        {
            var isAllowed = await this._simulationService.IsTokenValid(simulationId, simsimtoken);
            //this._simulationService.GetAgendaItems(simulationId);
            return null;
        }

        [Route("[action]")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<ResolutionSmallInfo>>> SimulationResolutions([FromHeader] string simsimtoken, int simulationId)
        {
            var context = _simulationService.GetDatabaseInstance();
            var validateToken = context.Simulations.Any(n => n.SimulationId == simulationId && n.Users.Any(a => a.Token == simsimtoken));
            if (!validateToken) return Forbid();
            var resolutions = await context.ResolutionAuths
                .Where(n => n.Simulation.SimulationId == simulationId)
                .Select(n => new ResolutionSmallInfo() { ResolutionId = n.ResolutionId, LastChangedTime = n.LastChangeTime, Name = n.Name}).ToListAsync();
            return Ok(resolutions);
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
        [AllowAnonymous]
        public async Task<ActionResult> MakePetition([FromBody] CreatePetitionRequest petition)
        {
            var user = this._simulationService.GetSimulationUser(petition.SimulationId, petition.Token);
            if (user == null) return Forbid();
            petition.PetitionUserId = user.SimulationUserId;
            var createdPetition = this._simulationService.SubmitPetition(petition);
            if (createdPetition != null)
            {
                await this._hubContext.Clients.Group($"sim_{petition.SimulationId}").UserPetition(createdPetition.ToPetitionDto());
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

        [HttpPost]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<SimulationTokenResponse>> JoinSimulation([FromBody]JoinAuthenticate request)
        {
            if (string.IsNullOrWhiteSpace(request.DisplayName)) return BadRequest();
            var simulation = await this._simulationService.GetSimulation(request.SimulationId);
            if (simulation == null) return NotFound();
            var user = this._simulationService.GetSimulationUserByPublicId(request.SimulationId, request.UserId);
            if (user == null) return NotFound();
            //if (simulation.Users.Any(n => n.DisplayName == request.DisplayName)) return BadRequest();
            if (user.Password != request.Password)
                return Forbid();

            if (!string.IsNullOrEmpty(request.DisplayName))
            {
                user.DisplayName = request.DisplayName;
                this._simulationService.SaveDbChanges();
            }
            if (string.IsNullOrEmpty(user.Token))
            {
                user.Token = Util.Tools.IdGenerator.RandomString(20);
                this._simulationService.SaveDbChanges();
            } 

            return Ok(user.AsTokenResponse());
        }

        [HttpPost]
        [Route("[action]")]
        [AllowAnonymous]
        public ActionResult<CreateSimulationResponse> CreateSimulation([FromBody]CreateSimulationRequest request)
        {
            var result = this._simulationService.CreateSimulation(request.Name, request.AdminPassword);
            var moderator = this._simulationService.CreateModerator(result, request.UserDisplayName ?? "");
            var admin = result.Users.First();
            var response = new CreateSimulationResponse()
            {
                FirstUserId = moderator.PublicUserId,
                FirstUserPassword = moderator.Password,
                FirstUserToken = moderator.Token,
                SimulationId = result.SimulationId,
                SimulationName = result.Name,
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> Subscribe([FromBody]SubscribeSimulation body)
        {
            if (string.IsNullOrEmpty(body.ConnectionId))
                return BadRequest();
            var user = this._simulationService.GetSimulationUser(body.SimulationId, body.Token);
            if (user == null) return Forbid();
            await this._hubContext.Groups.AddToGroupAsync(body.ConnectionId, $"sim_{body.SimulationId}");
            var mdl = new SimulationHubConnection()
            {
                User = user,
                ConnectionId = body.ConnectionId,
                CreationDate = DateTime.Now
            };
            user.HubConnections.Add(mdl);
            this._simulationService.SaveDbChanges();
            await this._hubContext.Clients.Group($"sim_{body.SimulationId}").UserConnected(body.SimulationId, user.AsUserItem());
            return Ok(true);
        }

        [HttpPost]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<CreatedVoteModel>> CreateVoting([FromBody]MUNity.Schema.Simulation.CreateSimulationVoting body)
        {
            var simulation = this._simulationService.GetSimulationWithHubsUsersAndRoles(body.SimulationId);
            if (simulation == null) return NotFound("Simulation not found!");
            var user = simulation.Users.FirstOrDefault(n => n.Token == body.Token);
            if (user == null || user.Role == null || user.Role.RoleType != RoleTypes.Chairman) return Forbid();
            var voting = new CreatedVoteModel()
            {
                CreatedVoteModelId = Guid.NewGuid().ToString(),
                Text = body.Text,
                AllowAbstention = body.AllowAbstention,
            };
            if (body.Mode == EVotingMode.Everyone)
                voting.AllowedUsers = simulation.Users.Where(n => n.HubConnections.Any()).Select(n => n.SimulationUserId).ToList();
            else if (body.Mode == EVotingMode.AllParticipants)
                voting.AllowedUsers = simulation.Users.Where(n => n.HubConnections.Any() && n.Role != null && n.Role.RoleType != RoleTypes.Chairman)
                    .Select(n => n.SimulationUserId).ToList();
            else if (body.Mode == EVotingMode.JustDelegates)
                voting.AllowedUsers = simulation.Users.Where(n => n.HubConnections.Any() && n.Role != null && n.Role.RoleType == RoleTypes.Delegate)
                    .Select(n => n.SimulationUserId).ToList();
            else if (body.Mode == EVotingMode.JustGuests)
                voting.AllowedUsers = simulation.Users.Where(n => n.HubConnections.Any() && n.Role != null && n.Role.RoleType == RoleTypes.Spectator)
                    .Select(n => n.SimulationUserId).ToList();
            else if (body.Mode == EVotingMode.JustNgos)
                voting.AllowedUsers = simulation.Users.Where(n => n.HubConnections.Any() && n.Role != null && n.Role.RoleType == RoleTypes.Ngo)
                    .Select(n => n.SimulationUserId).ToList();

            await this._hubContext.Clients.Group($"sim_{body.SimulationId}").VoteCreated(voting);
            return Ok(voting);
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult> Vote([FromHeader]string simsimtoken, int simulationId, string voteId, int choice)
        {
            var user = this._simulationService.GetSimulationUser(simulationId, simsimtoken);
            if (user == null) return Forbid();
            var args = new MUNity.Schema.Simulation.VotedEventArgs(voteId, user.SimulationUserId, choice);
            await this._hubContext.Clients.Group($"sim_{simulationId}").Voted(args);
            return Ok();
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<ActionResult> ApplyPetitionPreset([FromBody]ApplyPetitionTemplate body)
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
        public async Task<ActionResult<IEnumerable<PetitionTypeSimulationDto>>> SimulationPetitionTypes([FromHeader]string simsimtoken, int simulationId)
        {
            var isallowed = await this._simulationService.IsTokenValid(simulationId, simsimtoken);
            if (!isallowed) return Forbid();
            var types = this._simulationService.GetPetitionTypesOfSimulation(simulationId);
            var list = types.Select(n => n.ToDto());
            return Ok(list);
        }
    }
}