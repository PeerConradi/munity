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
using MUNityCore.Extensions;

namespace MUNityCore.Controllers
{

    /// <summary>
    /// The Controller for Simulations (online committees) etc.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SimulationController : ControllerBase, Simulation.ISimulationController
    {

        public IHubContext<Hubs.SimulationHub, MUNity.Hubs.ITypedSimulationHub> HubContext { get; set; }

        private readonly SimulationService _simulationService;

        public SimulationController(IHubContext<Hubs.SimulationHub, MUNity.Hubs.ITypedSimulationHub> hubContext,
            SimulationService simulationService)
        {
            this.HubContext = hubContext;
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

        [HttpGet]
        [Route("[action]")]
        public bool CanCreateSimulation()
        {
            return false;
        }

        [HttpGet]
        [Route("[action]")]
        public ActionResult<SimulationListItemDto> Info(int simulationId)
        {
            var instance = this._simulationService.GetDatabaseInstance();
            var sim = instance.Simulations.FirstOrDefault(n => n.SimulationId == simulationId);
            if (sim == null)
                return NotFound();
            var mdl = new SimulationListItemDto()
            {
                Name = sim.Name,
                Phase = sim.Phase,
                SimulationId = sim.SimulationId,
                UsingPassword = false
            };
            return Ok(mdl);
        }

        /// <summary>
        /// Returns a list of Simulations. Depricated
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public ActionResult<List<SimulationListItemDto>> GetListOfSimulations()
        {
            var result = this._simulationService.GetSimulations().Select(n => new SimulationListItemDto()
            {
                Name = n.Name,
                Phase = n.Phase,
                SimulationId = n.SimulationId,
                UsingPassword = !string.IsNullOrEmpty(n.Password)
            });
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
        public async Task<ActionResult<SimulationDto>> Simulation([FromHeader] string simsimtoken, int simulationId)
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
        public async Task<ActionResult<List<SimulationUserAdminDto>>> GetUsersAsAdmin([FromHeader] string simsimtoken, int id)
        {
            var isAllowed = await _simulationService.IsTokenValidAndUserChairOrOwner(id, simsimtoken);
            if (!isAllowed) return Forbid();
            var users = this._simulationService.GetSimulationUsers(id);
            users.Include(n => n.Role)
                .Include(n => n.HubConnections)
                .Include(n => n.Simulation).ToList();
            var result = users.Select(n => n.ToSimulationUserAdminDto()).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<bool>> IsUserOnline([FromHeader] string simsimtoken, int simulationId, int userId)
        {
            var isAllowed = await this._simulationService.IsTokenValid(simulationId, simsimtoken);
            if (!isAllowed) return Forbid();
            return Ok(this._simulationService.UserOnline(simulationId, userId));
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<SimulationUserDefaultDto>> GetUsersDefault([FromHeader] string simsimtoken, int id)
        {
            var isAllowed = await this._simulationService.IsTokenValid(id, simsimtoken);
            if (!isAllowed) return Forbid();
            var users = this._simulationService.GetSimulationUsers(id);
            users.Include(n => n.Role)
                .Include(n => n.HubConnections)
                .Include(n => n.Simulation).ToList();
            var result = users.Select(n => n.AsSimulationUserDefaultDto());
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public ActionResult<SimulationAuthDto> GetSimulationAuth([FromHeader] string simsimtoken, int id)
        {
            var user = this._simulationService.GetSimulationUser(id, simsimtoken);
            if (user == null) return NotFound();
            return Ok(user.ToSimulationAuthDto());
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult> PickRole([FromHeader] string simsimtoken, int simulationId, int roleId)
        {
            var simulation = await this._simulationService.GetSimulationWithUsersAndRoles(simulationId);
            if (simulation == null) return NotFound();
            var user = simulation.Users.FirstOrDefault(n => n.Token == simsimtoken);
            if (user == null) return NotFound();
            if (!user.CanSelectRole) return Forbid();
            var role = simulation.Roles.FirstOrDefault(n => n.SimulationRoleId == roleId);
            if (role == null) return NotFound();
            this._simulationService.BecomeRole(simulation, user, role);
            await this.HubContext.Clients.Group($"sim_{simulationId}").UserRoleChanged(new UserRoleChangedEventArgs(simulationId, user.SimulationUserId, roleId));
            return Ok();
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult> ApplyPreset([FromHeader] string simsimtoken, int simulationId, string presetId)
        {
            var simulation = await this._simulationService.GetSimulationWithUsersAndRoles(simulationId);
            if (simulation == null) return NotFound();
            if (!simulation.Users.Any(n => n.Token == simsimtoken && n.CanCreateRole)) return Forbid();
            var preset = this._simulationService.Presets.FirstOrDefault(n => n.Id == presetId);
            if (preset == null) return NotFound();
            this._simulationService.ApplyPreset(simulation, preset);
            await this.HubContext.Clients.Group($"sim_{simulationId}").RolesChanged(new RolesChangedEventArgs(simulationId, simulation.Roles.Select(n => n.ToSimulationRoleDto())));
            return Ok();
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult> RemoveSimulationUser([FromHeader] string simsimtoken, int simulationId, int userId)
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
        public async Task<ActionResult> SetPhase([FromBody] SetPhaseRequest body)
        {
            var isAllowed = await this._simulationService.IsTokenValidAndUserChairOrOwner(body);
            if (!isAllowed) return Forbid();
            var hasChanged = await this._simulationService.SetPhase(body.SimulationId, body.SimulationPhase);
            if (hasChanged)
            {
                _ = this.HubContext.Clients.Group($"sim_{body.SimulationId}").PhaseChanged(body.SimulationId, body.SimulationPhase);
            }
            return Ok();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<ResolutionSmallInfo>> CreateResolution([FromServices]IResolutionService resaService, [FromBody]SimulationRequest body)
        {
            var isAllowed = await this._simulationService.IsTokenValidAndUserChairOrOwner(body);
            if (!isAllowed) return Forbid();

            var resolution = await resaService.CreateSimulationResolution(body.SimulationId);
            var smallInfo = new ResolutionSmallInfo()
            {
                LastChangedTime = resolution.LastChangeTime,
                Name = resolution.Name,
                ResolutionId = resolution.ResolutionId
            };
            return Ok(smallInfo);
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

            return Ok(user.ToTokenResponse());
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
            await this.HubContext.Groups.AddToGroupAsync(body.ConnectionId, $"sim_{body.SimulationId}");
            var mdl = new SimulationHubConnection()
            {
                User = user,
                ConnectionId = body.ConnectionId,
                CreationDate = DateTime.Now
            };
            user.HubConnections.Add(mdl);
            this._simulationService.SaveDbChanges();
            await this.HubContext.Clients.Group($"sim_{body.SimulationId}").UserConnected(body.SimulationId, user.AsSimulationUserDefaultDto());
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

            await this.HubContext.Clients.Group($"sim_{body.SimulationId}").VoteCreated(voting);
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
            await this.HubContext.Clients.Group($"sim_{simulationId}").Voted(args);
            return Ok();
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<SimulationSlotDto>>> Slots([FromHeader]string simsimtoken, int simulationId)
        {
            var isAllowed = await this._simulationService.IsTokenValid(simulationId, simsimtoken);
            if (!isAllowed) Forbid();

            List<SimulationSlotDto> slots = this._simulationService.GetSlots(simulationId);
            return Ok(slots);
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<ActionResult> CloseAllConnections([FromBody]SimulationRequest request)
        {
            var isAllowed = await this._simulationService.IsTokenValidAndUserAdmin(request);
            if (!isAllowed)
                isAllowed = await this._simulationService.IsTokenValidAndUserChair(request);
            if (!isAllowed) return Forbid();

            List<SimulationHubConnection> hubConnections = this._simulationService.GetHubConnections(request.SimulationId);
            foreach(var connection in hubConnections)
            {
                await this.HubContext.Groups.RemoveFromGroupAsync(connection.ConnectionId, $"sim_{request.SimulationId}");
                this._simulationService.RemoveHubConnection(connection);
            }
            return Ok();
        }
        
    }
}