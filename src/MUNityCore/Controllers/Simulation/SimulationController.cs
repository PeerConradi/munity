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
using Microsoft.AspNetCore.Cors;

namespace MUNityCore.Controllers
{

    /// <summary>
    /// The Controller for Simulations (online committees) etc.
    /// Note that some of this methods may be deprecated
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

        /// <summary>
        /// Checks if new Simulations can be created.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public bool CanCreateSimulation()
        {
            return true;
        }

        /// <summary>
        /// Returns an info Element for the simulation with the given id.
        /// </summary>
        /// <param name="simulationId"></param>
        /// <returns></returns>
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
            if (!isAllowed) return BadRequest();

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
            if (!isValid) return BadRequest();
            return this._simulationService.GetSpeakerlistIdOfSimulation(simulationId);
        }

        /// <summary>
        /// Returns an id for the resolution that is currently worked on inside the simulation.
        /// </summary>
        /// <param name="simsimtoken"></param>
        /// <param name="simulationId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> GetResolutionId([FromHeader] string simsimtoken, int simulationId)
        {
            var isValid = await this._simulationService.IsTokenValid(simulationId, simsimtoken);
            if (!isValid) return BadRequest();
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
            if (!isAllowed) return BadRequest();
            // TODO: Send Socket information.
            return Ok(this._simulationService.InitListOfSpeakers(simulationId));
        }

        /// <summary>
        /// Returns the list of users/slots with the Admin Informations (id and access-codes)
        /// </summary>
        /// <param name="simsimtoken"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<List<SimulationUserAdminDto>>> GetUsersAsAdmin([FromHeader] string simsimtoken, int id)
        {
            var isAllowed = await _simulationService.IsTokenValidAndUserChairOrOwner(id, simsimtoken);
            if (!isAllowed) return BadRequest();
            var users = this._simulationService.GetSimulationUsers(id);
            users.Include(n => n.Role)
                .Include(n => n.HubConnections)
                .Include(n => n.Simulation).ToList();
            var result = users.Select(n => n.ToSimulationUserAdminDto()).ToList();
            return Ok(result);
        }

        /// <summary>
        /// Checks if a user is online (someone is taking this slot and has an active connection to the WebSocket)
        /// </summary>
        /// <param name="simsimtoken"></param>
        /// <param name="simulationId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<bool>> IsUserOnline([FromHeader] string simsimtoken, int simulationId, int userId)
        {
            var isAllowed = await this._simulationService.IsTokenValid(simulationId, simsimtoken);
            if (!isAllowed) return BadRequest();
            return Ok(this._simulationService.UserOnline(simulationId, userId));
        }

        /// <summary>
        /// Gets the list of users. You should think about using the Slot <see cref="Slots(string, int)"/> method instead
        /// since it will give you more information.
        /// </summary>
        /// <param name="simsimtoken"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<SimulationUserDefaultDto>> GetUsersDefault([FromHeader] string simsimtoken, int id)
        {
            var isAllowed = await this._simulationService.IsTokenValid(id, simsimtoken);
            if (!isAllowed) return BadRequest();
            var users = this._simulationService.GetSimulationUsers(id);
            users.Include(n => n.Role)
                .Include(n => n.HubConnections)
                .Include(n => n.Simulation).ToList();
            var result = users.Select(n => n.AsSimulationUserDefaultDto());
            return Ok(result);
        }

        /// <summary>
        /// Gets the auth of the simsimtoken you pass to this method inside the request header inside the
        /// simulation with the given id.
        /// </summary>
        /// <param name="simsimtoken"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public ActionResult<SimulationAuthDto> GetSimulationAuth([FromHeader] string simsimtoken, int id)
        {
            if (!string.IsNullOrEmpty(Program.MasterToken) && simsimtoken == Program.MasterToken)
            {
                return new SimulationAuthDto()
                {
                    SimulationUserId = -2,
                    CanCreateRole = false,
                    CanEditListOfSpeakers = false,
                    CanEditResolution = false,
                    CanSelectRole = false
                };
            }
            var user = this._simulationService.GetSimulationUser(id, simsimtoken);
            if (user == null) return NotFound();
            return Ok(user.ToSimulationAuthDto());
        }

        /// <summary>
        /// Lets the user pick a role for his/her slot. This will only work if the user is allowed
        /// to change the role.
        /// </summary>
        /// <param name="simsimtoken"></param>
        /// <param name="simulationId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult> PickRole([FromHeader] string simsimtoken, int simulationId, int roleId)
        {
            var simulation = await this._simulationService.GetSimulationWithUsersAndRoles(simulationId);
            if (simulation == null) return NotFound();
            var user = simulation.Users.FirstOrDefault(n => n.Token == simsimtoken);
            if (user == null) return NotFound();
            if (!user.CanSelectRole) return BadRequest();
            var role = simulation.Roles.FirstOrDefault(n => n.SimulationRoleId == roleId);
            if (role == null) return NotFound();
            this._simulationService.BecomeRole(simulation, user, role);
            await this.HubContext.Clients.Group($"sim_{simulationId}").UserRoleChanged(new UserRoleChangedEventArgs(simulationId, user.SimulationUserId, roleId));
            return Ok();
        }

        /// <summary>
        /// Applies a preset for simulation roles.
        /// </summary>
        /// <param name="simsimtoken"></param>
        /// <param name="simulationId"></param>
        /// <param name="presetId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult> ApplyPreset([FromHeader] string simsimtoken, int simulationId, string presetId)
        {
            var simulation = await this._simulationService.GetSimulationWithUsersAndRoles(simulationId);
            if (simulation == null) return NotFound();
            if (!simulation.Users.Any(n => n.Token == simsimtoken && n.CanCreateRole)) return BadRequest();
            var preset = this._simulationService.Presets.FirstOrDefault(n => n.Id == presetId);
            if (preset == null) return NotFound();
            this._simulationService.ApplyPreset(simulation, preset);
            await this.HubContext.Clients.Group($"sim_{simulationId}").RolesChanged(new RolesChangedEventArgs(simulationId, simulation.Roles.Select(n => n.ToSimulationRoleDto())));
            return Ok();
        }

        /// <summary>
        /// Removes a Slot from the simulation.
        /// </summary>
        /// <param name="simsimtoken"></param>
        /// <param name="simulationId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult> RemoveSimulationUser([FromHeader] string simsimtoken, int simulationId, int userId)
        {
            var simulation = await this._simulationService.GetSimulationWithUsersAndRoles(simulationId);
            if (simulation == null) return NotFound();
            if (!simulation.Users.Any(n => n.Token == simsimtoken && n.CanCreateRole)) return BadRequest();
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

        /// <summary>
        /// Changes the phase of the simulation
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<ActionResult> SetPhase([FromBody] SetPhaseRequest body)
        {
            var isAllowed = await this._simulationService.IsTokenValidAndUserChairOrOwner(body);
            if (!isAllowed) return BadRequest();
            var hasChanged = await this._simulationService.SetPhase(body.SimulationId, body.SimulationPhase);
            if (hasChanged)
            {
                _ = this.HubContext.Clients.Group($"sim_{body.SimulationId}").PhaseChanged(body.SimulationId, body.SimulationPhase);
            }
            return Ok();
        }

        /// <summary>
        /// Creates a new resolution and links this created resolution to the simulation.
        /// </summary>
        /// <param name="resaService"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public ActionResult<ResolutionSmallInfo> CreateResolution([FromServices]SqlResolutionService resaService, [FromBody]CreateSimulationResolutionRequest body)
        {
            var user = _simulationService.GetSimulationUserWithRole(body.SimulationId, body.Token);
            if (user == null) return BadRequest();
            //var isAllowed = await this._simulationService.IsTokenValidAndUserChairOrOwner(body);
            //if (!isAllowed) return BadRequest();

            var submitter = "";
            if (user.Role != null) submitter = user.Role.Name;
            submitter = user.DisplayName;

            var resolution = resaService.CreateSimulationResolution(body, submitter);
            var smallInfo = new ResolutionSmallInfo()
            {
                LastChangedTime = resolution.LastChangeTime,
                Name = resolution.Name,
                ResolutionId = resolution.ResolutionId,
                AllowAmendments = resolution.AllowOnlineAmendments,
                AllowPublicEdit = resolution.AllowPublicEdit
            };
            return Ok(smallInfo);
        }

        /// <summary>
        /// unlinks a resolution from the simulation. This will change the resolution back to public or any other mode
        /// that was applied to it before.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> UnlinkResolution([FromBody] MUNity.Schema.Simulation.Resolution.SimulationResolutionRequest body)
        {
            var isAllowed = await this._simulationService.IsTokenValidAndUserChairOrOwner(body);
            if (!isAllowed)
                return BadRequest();

            var success = this._simulationService.UnlinkResolution(body.ResolutionId);
            if (!success)
                return NotFound();

            return Ok();
        }

        /// <summary>
        /// Returns a list of information of resolutions that are linked to the given simulation.
        /// You need to pass the token of the user inside the simsimtoken header.
        /// </summary>
        /// <param name="simsimtoken"></param>
        /// <param name="simulationId"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<ResolutionSmallInfo>>> SimulationResolutions([FromHeader] string simsimtoken, int simulationId)
        {
            var context = _simulationService.GetDatabaseInstance();
            var isAllowed = await this._simulationService.IsTokenValid(simulationId, simsimtoken);

            if (!isAllowed) return BadRequest();
            var resolutions = from auth in context.ResolutionAuths
                              where auth.Simulation.SimulationId == simulationId
                              join resa in context.Resolutions on auth.ResolutionId equals resa.ResaElementId
                              select new ResolutionSmallInfo()
                              {
                                  AllowAmendments = auth.AllowOnlineAmendments,
                                  AllowPublicEdit = auth.AllowPublicEdit,
                                  LastChangedTime = resa.CreatedDate,
                                  Name = resa.Topic,
                                  ResolutionId = resa.ResaElementId
                              };
            var result = await resolutions.ToListAsync();
            return Ok(result);
        }

        /// <summary>
        /// Lets a user with given id, password and a display name join into the slot that
        /// is matching the given id/password criteria.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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
                return BadRequest();

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

        /// <summary>
        /// Creates a new simulation room and creates the first slot inside this simulation and will return
        /// the needed access codes for this first slot.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Subscribes the given connectionId to the simulationSocket. Everytime something
        /// inside the simulation changes the user will be informed via the socket. 
        /// Note that the socket is interrupted/reconnected you may need to resubscribe.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> Subscribe([FromBody]SubscribeSimulation body)
        {
            if (string.IsNullOrEmpty(body.ConnectionId))
                return BadRequest();
            var allowed = await this._simulationService.IsTokenValid(body);
            if (!allowed) 
                return BadRequest();

            var user = this._simulationService.GetSimulationUser(body.SimulationId, body.Token);
            await this.HubContext.Groups.AddToGroupAsync(body.ConnectionId, $"sim_{body.SimulationId}");
            

            if (user != null)
            {
                await this._simulationService.AddUserSubscribtion(user, body.ConnectionId);
                await this.HubContext.Clients.Group($"sim_{body.SimulationId}").UserConnected(body.SimulationId, user.AsSimulationUserDefaultDto());
            }
            
            return Ok(true);
        }

        /// <summary>
        /// Returns the slots of the simulation. This will return every slot with the current user that is inside the slot
        /// and the role that is applied to the slot.
        /// </summary>
        /// <param name="simsimtoken"></param>
        /// <param name="simulationId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<SimulationSlotDto>>> Slots([FromHeader]string simsimtoken, int simulationId)
        {
            var isAllowed = await this._simulationService.IsTokenValid(simulationId, simsimtoken);
            if (!isAllowed) BadRequest();

            List<SimulationSlotDto> slots = this._simulationService.GetSlots(simulationId);
            var offlineSlots = slots.Where(n => n.IsOnline == false);
            if (offlineSlots != null && offlineSlots.Any())
            {
                foreach (var offline in offlineSlots)
                {
                    var userInDb = this._simulationService.GetSimulationUser(offline.SimulationUserId);
                    if (userInDb != null)
                    {
                        if (Hubs.ConnectionUsers.ConnectionIds != null)
                        {
                            offline.IsOnline = Hubs.ConnectionUsers.ConnectionIds.Any(n => n.Key == userInDb.LastKnownConnectionId && !string.IsNullOrEmpty(userInDb.LastKnownConnectionId));
                        }
                        
                    }
                }
            }
            
            return Ok(slots);
        }

        /// <summary>
        /// Removes all Connections from the simulation socket.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<ActionResult> CloseAllConnections([FromBody]SimulationRequest request)
        {
            var isAllowed = await this._simulationService.IsTokenValidAndUserAdmin(request);
            if (!isAllowed)
                isAllowed = await this._simulationService.IsTokenValidAndUserChair(request);
            if (!isAllowed) return BadRequest();

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