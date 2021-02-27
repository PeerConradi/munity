using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using MUNity.Models.Simulation;
using MUNity.Schema.Simulation;
using MUNityCore.DataHandlers.EntityFramework;
using MUNityCore.Extensions.CastExtensions;
using MUNityCore.Models.Simulation;
using MUNityCore.Models.Simulation.Presets;

namespace MUNityCore.Services
{

    /// <summary>
    /// A service for virtual committees.
    /// The service should be handled as a singleton, because the virtual committee is created and destroied within
    /// the Memory.
    /// </summary>
    public class SimulationService
    {

        private readonly MunityContext _context;

        public IEnumerable<Models.Simulation.Presets.ISimulationPreset> Presets
        {
            get
            {
                yield return new Models.Simulation.Presets.PresetSicherheitsrat();
                yield return new Models.Simulation.Presets.PresetGV_TVT();
                yield return new Models.Simulation.Presets.PresetMR_TVT();
            }
        }

        internal SimulationStatus SetStatus(SetSimulationStatusDto body)
        {
            var simulation = _context.Simulations.FirstOrDefault(n => n.SimulationId == body.SimulationId);
            if (simulation == null) return null;
            var status = new SimulationStatus()
            {
                Simulation = simulation,
                StatusText = body.StatusText,
                StatusTime = DateTime.Now
            };
            _context.SimulationStatuses.Add(status);
            this._context.SaveChanges();
            return status;
        }

        internal SimulationRole CreateRole(CreateRoleRequest body)
        {
            var simulation = _context.Simulations.FirstOrDefault(n => n.SimulationId == body.SimulationId);
            if (simulation == null) return null;

            var newRole = new SimulationRole(body.Iso, body.Name, body.RoleType);
            newRole.Simulation = simulation;
            _context.SimulationRoles.Add(newRole);
            _context.SaveChanges();
            return newRole;
        }

        public void RemoveHubs(IEnumerable<SimulationHubConnection> hubs)
        {
            try
            {
                this._context.SimulationHubConnections.RemoveRange(hubs);
                this._context.SaveChanges();
            }
            catch (Exception)
            {
                // TODO: Logger
            }
        }

        internal PetitionType CreatePetitionType(CreatePetitionTypeRequest body)
        {
            var newPetitionType = new PetitionType()
            {
                Category = body.Category,
                Description = body.Description,
                Name = body.Name,
                Reference = body.Reference,
                Ruling = body.Ruling
            };
            _context.PetitionTypes.Add(newPetitionType);
            _context.SaveChanges();
            return newPetitionType;
        }

        internal bool SetUserRole(SetUserSimulationRole body)
        {
            var role = _context.SimulationRoles.FirstOrDefault(n => n.SimulationRoleId == body.RoleId);
            if (role == null) return false;
            var user = _context.SimulationUser.FirstOrDefault(n => n.SimulationUserId == body.UserId);
            if (user == null) return false;
            user.Role = role;
            _context.SaveChanges();
            return true;
        }

        public Simulation CreateSimulation(string name, string password)
        {
            var sim = new Simulation()
            {
                Name = name,
                Password = password
            };

            this._context.Simulations.Add(sim);
            this._context.SaveChanges();
            return sim;
        }

        public string GetSpeakerlistIdOfSimulation(int simulationId)
        {
            return this._context.Simulations.Include(n => n.ListOfSpeakers).FirstOrDefault(n => n.SimulationId == simulationId)?.ListOfSpeakers?.ListOfSpeakersId;
        }

        internal Simulation GetSimulationAndUserByConnectionId(string connectionId)
        {
            return _context.Simulations
                .Include(n => n.Users)
                .ThenInclude(n => n.HubConnections)
                .FirstOrDefault(n => 
                    n.Users.Any(k => k.HubConnections.Any(a => a.ConnectionId == connectionId)));
        }

        public SimulationUser CreateModerator(Simulation simulation, string displayName)
        {
            var ownerUser = new SimulationUser()
            {
                CanCreateRole = true,
                CanEditListOfSpeakers = true,
                CanEditResolution = true,
                CanSelectRole = true,
                DisplayName = displayName,
                Role = null,
                Simulation = simulation,
            };
            _context.SimulationUser.Add(ownerUser);
            _context.SaveChanges();
            return ownerUser;
        }

        internal SimulationDto GetSimulationResponse(int id)
        {
            var simulation = GetSimulationWithHubsUsersAndRoles(id);
            return simulation.ToSimulationDto();
        }

        public SimulationUser CreateUser(int simulationId, string displayName)
        {
            var baseUser = new SimulationUser()
            {
                CanCreateRole = false,
                CanEditListOfSpeakers = false,
                CanEditResolution = false,
                CanSelectRole = false,
                DisplayName = displayName,
                Role = null
            };
            // I have no idea why this is necessary but this will fix
            // that the users are suddenly empty...
            var users = _context.SimulationUser.Where(n => n.Simulation.SimulationId == simulationId).ToList();
            var simulation = _context.Simulations.Include(n => n.Users).FirstOrDefault(n => n.SimulationId == simulationId);
            simulation.Users.AddRange(users);
            baseUser.Simulation = simulation;
            _context.SimulationUser.Add(baseUser);
            _context.SaveChanges();
            return baseUser;
        }

        internal bool RemovePetition(PetitionInteractRequest body)
        {
            var petition = _context.Petitions.FirstOrDefault(n => n.PetitionId == body.PetitionId);
            if (petition == null) return false;
            _context.Petitions.Remove(petition);
            int changes = _context.SaveChanges();
            return changes == 1;
        }

        internal async Task<bool> IsPetitionInteractionAllowed(PetitionInteractRequest body)
        {
            var petition = this._context.Petitions.Include(n => n.SimulationUser).FirstOrDefault(n => n.PetitionId == body.PetitionId);
            var isUsersPetition = petition.SimulationUser.Token == body.Token;
            if (isUsersPetition) return true;
            return await IsTokenValidAndUserChair(body);
        }

        public Task<Simulation> GetSimulation(int id)
        {
            return this._context.Simulations.FirstOrDefaultAsync(n => n.SimulationId == id);
        }

        

        

        public Simulation GetSimulationWithHubsUsersAndRoles(int id)
        {
            var simulation = this._context.Simulations
                .Include(n => n.Roles).FirstOrDefault(n => n.SimulationId == id);
            if (simulation == null) return null;
            var users = this._context.SimulationUser
                .Include(n => n.HubConnections).Where(a => a.Simulation.SimulationId == id).AsEnumerable().Where(n => n.HubConnections.Any());
            simulation.Users = users.ToList();
            return simulation;
        }

        public IEnumerable<Simulation> GetSimulations()
        {
            return this._context.Simulations.AsEnumerable();
        }


        //public IQueryable<SimulationRole> GetSimulationsRoles(int simulationId)
        //{
        //    return this._context.SimulationRoles.Where(n => n.Simulation.SimulationId == simulationId);
        //}

        public IQueryable<SimulationUser> GetSimulationUsers(int simulationId)
        {
            return this._context.SimulationUser.Where(n => n.Simulation.SimulationId == simulationId);
        }

        public SimulationUser GetSimulationUserByPublicId(int simulationId, string publicId)
        {
            return this._context.SimulationUser.FirstOrDefault(n => n.Simulation.SimulationId == simulationId && n.PublicUserId == publicId);
        }

        public bool UserOnline(int simulationId, int userId)
        {
            return this._context.SimulationUser.Any(n => n.Simulation.SimulationId == simulationId && n.SimulationUserId == userId);
        }

        public SimulationUser GetSimulationUser(int simulationId, string token)
        {
            return this._context.SimulationUser.FirstOrDefault(n => n.Simulation.SimulationId == simulationId && n.Token == token);
        }

        public int? GetSimulationUserId(int simulationId, string token)
        {
            return this._context.SimulationUser.FirstOrDefault(n => n.Simulation.SimulationId == simulationId && n.Token == token)?.SimulationUserId;
        }
        

        public MUNity.Models.ListOfSpeakers.ListOfSpeakers InitListOfSpeakers(int simulationId)
        {
            var simulation = this._context.Simulations.Include(n => n.ListOfSpeakers).FirstOrDefault(n => n.SimulationId == simulationId);
            if (simulation == null) return null;
            simulation.ListOfSpeakers = new MUNity.Models.ListOfSpeakers.ListOfSpeakers();
            this._context.ListOfSpeakers.Add(simulation.ListOfSpeakers);
            this._context.SaveChanges();
            return simulation.ListOfSpeakers;
        }

        public void SaveDbChanges()
        {
            this._context.SaveChanges();
        }

        

        internal void ApplyPreset(Simulation simulation, ISimulationPreset preset, bool removeExistingRoles = true)
        {
            if (removeExistingRoles) simulation.Roles.Clear();
            simulation.Roles.AddRange(preset.Roles);
            this._context.SaveChanges();
        }



        #region roles

        public SimulationUser GetSimulationUserWithRole(int simulationId, string token)
        {
            return this._context.SimulationUser.Include(n => n.Role).FirstOrDefault(n => n.Simulation.SimulationId == simulationId && n.Token == token);
        }

        public SimulationRole AddChairmanRole(int simulationid, string name)
        {
            var simulation = this._context.Simulations
                .Include(n => n.Roles)
                .FirstOrDefault(n => n.SimulationId == simulationid);

            var currentChairmanRole = simulation.Roles.FirstOrDefault(n =>
                n.RoleType == RoleTypes.Chairman);
            if (currentChairmanRole == null)
            {
                currentChairmanRole = new SimulationRole()
                {
                    RoleType = RoleTypes.Chairman,
                    Iso = "UN"
                };
                simulation.Roles.Add(currentChairmanRole);
            }

            currentChairmanRole.Name = name;
            this._context.SaveChanges();
            return currentChairmanRole;
        }

        public Petition SubmitPetition(CreatePetitionRequest dto)
        {
            var agendaItem = this._context.AgendaItems.Include(n => n.Petitions).FirstOrDefault(n => n.AgendaItemId == dto.TargetAgendaItemId);
            if (agendaItem == null) throw new Exception("Agenda Item Not found");

            var petitionType = this._context.PetitionTypes.FirstOrDefault(n => n.PetitionTypeId == dto.PetitionTypeId);
            if (petitionType == null) throw new Exception("Petition not found");

            var user = this._context.SimulationUser.FirstOrDefault(n => n.SimulationUserId == dto.PetitionUserId);
            if (user == null) throw new Exception("User not found!");

            var newItem = new Petition()
            {
                AgendaItem = agendaItem,
                PetitionDate = DateTime.Now,
                PetitionType = petitionType,
                SimulationUser = user,
                Status = dto.Status,
                Text = dto.Text
            };
            agendaItem.Petitions.Add(newItem);
            _context.SaveChanges();
            return newItem;
        }

        public async Task<bool> SetPhase(int simulationId, GamePhases phase)
        {
            var simulation = await this._context.Simulations.FirstOrDefaultAsync(n => n.SimulationId == simulationId);
            if (simulation == null) return false;
            simulation.Phase = phase;
            await this._context.SaveChangesAsync();
            return true;
        }

        public SimulationRole AddDelegateRole(int simulationId, string name, string iso)
        {
            var simulation = this._context.Simulations.FirstOrDefault(n => n.SimulationId == simulationId);
            if (simulation == null) return null;
            var role = new SimulationRole()
            {
                Iso = iso,
                Name = name,
                RoleType = RoleTypes.Delegate,
                Simulation = simulation,
            };
            _context.SimulationRoles.Add(role);
            this._context.SaveChanges();
            return role;
        }

        public bool BecomeRole(Simulation simulation, SimulationUser user, SimulationRole role)
        {
            var users = this._context.SimulationUser
                .Include(n => n.Role)
                .Where(n => n.Simulation.SimulationId == simulation.SimulationId);

            // Cannot take this role because it is already taken!
            // if (users.Any(n => n.Role.SimulationRoleId == role.SimulationRoleId)) return false;

            user.Role = role;
            this._context.SaveChanges();
            return true;
        }

        public Task<List<SimulationRole>> GetSimulationRoles(int simulationId)
        {
            return this._context.SimulationRoles.Where(n => n.Simulation.SimulationId == simulationId).ToListAsync();
        }

        #endregion

        public SimulationService(MunityContext context)
        {
            this._context = context;
        }

        public Task<List<Models.Simulation.PetitionType>> GetPetitionTypes()
        {
            return this._context.PetitionTypes.ToListAsync();
        }

        #region Validation

        public Task<bool> IsTokenValidAndUserAdmin(SimulationRequest requestSchema)
        {
            return _context.SimulationUser.AnyAsync(n => 
            n.Simulation.SimulationId == requestSchema.SimulationId && 
            n.Token == requestSchema.Token &&
            n.CanCreateRole);
        }

        public async Task<bool> IsTokenValidAndUserAdmin(int simulationId, string token)
        {
            return await _context.SimulationUser.AnyAsync(n =>
            n.Simulation.SimulationId == simulationId &&
            n.Token == token &&
            n.CanCreateRole);
        }

        internal Task<bool> IsTokenValid(SimulationRequest requestSchema)
        {
            return _context.SimulationUser.AnyAsync(n =>
            n.Simulation.SimulationId == requestSchema.SimulationId &&
            n.Token == requestSchema.Token);
        }

        public Task<bool> IsTokenValid(int simulationId, string token)
        {
            return _context.SimulationUser.AnyAsync(n =>
            n.Simulation.SimulationId == simulationId &&
            n.Token == token);
        }

        public Task<bool> IsTokenValidAndUserDelegate(SimulationRequest requestSchema)
        {
            return _context.SimulationUser.AnyAsync(n =>
            n.Simulation.SimulationId == requestSchema.SimulationId &&
            n.Token == requestSchema.Token &&
            n.Role != null &&
            n.Role.RoleType == RoleTypes.Delegate);
        }

        public Task<bool> IsTokenValidAndUserChair(SimulationRequest requestSchema)
        {
            return _context.SimulationUser.AnyAsync(n =>
            n.Simulation.SimulationId == requestSchema.SimulationId &&
            n.Token == requestSchema.Token &&
            n.Role != null &&
            n.Role.RoleType == RoleTypes.Chairman);
        }

        public Task<bool> IsTokenValidAndUserChair(int simulationId, string token)
        {
            return _context.SimulationUser.AnyAsync(n =>
            n.Simulation.SimulationId == simulationId &&
            n.Token == token &&
            n.Role != null &&
            n.Role.RoleType == RoleTypes.Chairman);
        }

        public Task<bool> IsTokenValidAndUserChairOrOwner(int simulationId, string token)
        {
            return _context.SimulationUser.AnyAsync(n =>
            n.Simulation.SimulationId == simulationId &&
            n.Token == token &&
            (n.CanCreateRole ||
            n.Role != null &&
            n.Role.RoleType == RoleTypes.Chairman));
        }

        public Task<bool> IsTokenValidAndUserChairOrOwner(SimulationRequest request)
        {
            return IsTokenValidAndUserChairOrOwner(request.SimulationId, request.Token);
        }

        #endregion

        #region Petitions

        public async Task<bool> AddPetitionTypeToSimulation(MUNity.Schema.Simulation.Managment.AddPetitionTypeRequestBody request)
        {
            var simulation = await _context.Simulations.Include(n => n.PetitionTypes)
                .ThenInclude(n => n.PetitionType)
                .FirstOrDefaultAsync(n => n.SimulationId == request.SimulationId);
            if (simulation == null) return false;
            var petitionTypeToAdd = await _context.PetitionTypes.FirstOrDefaultAsync(n => n.PetitionTypeId == request.PetitionTypeId);
            if (petitionTypeToAdd == null) return false;
            if (simulation.PetitionTypes.Any(n => n.PetitionType.PetitionTypeId == request.PetitionTypeId)) return true;   //already inside
            var newEntry = new PetitionTypeSimulation()
            {
                AllowChairs = request.AllowChairs,
                AllowDelegates = request.AllowDelegates,
                AllowNgo = request.AllowNgo,
                AllowSpectator = request.AllowSpectator,
                OrderIndex = (request.OrderIndex != -1) ? request.OrderIndex : simulation.PetitionTypes.Count,
                PetitionType = petitionTypeToAdd,
                Simulation = simulation
            };
            simulation.PetitionTypes.Add(newEntry);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<AgendaItem> CreateAgendaItem(CreateAgendaItemDto agendaItem)
        {
            
            var simulation = this._context.Simulations.Include(n => n.AgendaItems).FirstOrDefault(n => n.SimulationId == agendaItem.SimulationId);
            if (simulation == null) return null;
            var hasAnyElements = simulation.AgendaItems.Any();
            if (hasAnyElements)
            {
                var hasMatchingAgendaItem = simulation.AgendaItems.Any(n =>
                (n.Name == agendaItem.Name));
                if (hasMatchingAgendaItem) return null;
            }
            
            var item = new AgendaItem(agendaItem);

            if (hasAnyElements)
            {
                item.OrderIndex = simulation.AgendaItems.Max(n => n.OrderIndex) + 1;
            }
            else
            {
                item.OrderIndex = 1;
            }

            simulation.AgendaItems.Add(item);
            _context.AgendaItems.Add(item);
            await this._context.SaveChangesAsync();
            return item;
        }

        public Task<List<Models.Simulation.PetitionType>> GetSimulationPetitionTypes(int simulationId)
        {
            return _context.SimulationPetitionTypes.Where(n => n.Simulation.SimulationId == simulationId)
                .Include(n => n.PetitionType).Select(n => n.PetitionType).ToListAsync();
        }

        internal List<SimulationSlotDto> GetSlots(int simulationId)
        {
            var t = from user in _context.SimulationUser
                    where user.Simulation.SimulationId == simulationId
                    select new SimulationSlotDto()
                    {
                        CanCreateRole = user.CanCreateRole,
                        CanEditListOfSpeakers = user.CanEditListOfSpeakers,
                        CanEditResolution = user.CanEditResolution,
                        CanSelectRole = user.CanSelectRole,
                        DisplayName = user.DisplayName,
                        IsOnline = user.HubConnections.Any(),
                        RoleId = (user.Role != null) ? user.Role.SimulationRoleId : -2,
                        RoleName = (user.Role != null) ? user.Role.Name : "",
                        RoleType = (user.Role != null) ? user.Role.RoleType : RoleTypes.None,
                        RoleIso = (user.Role != null) ? user.Role.Iso : "",
                        SimulationUserId = user.SimulationUserId
                    };
            return t.ToList();
        }

        #endregion

        #region User Managment

        public Task<Simulation> GetSimulationWithUsersAndRoles(int id)
        {
            return this._context.Simulations
                .Include(n => n.Roles)
                .Include(n => n.Users)
                .FirstOrDefaultAsync(n => n.SimulationId == id);
        }

        public async Task<List<AgendaItemDto>> GetAgendaItemsAndPetitionsDto(int simulationId)
        {
            var test = _context.AgendaItems
                .Where(n => n.Simulation.SimulationId == simulationId)
                .Select(n => new AgendaItemDto()
                {
                    AgendaItemId = n.AgendaItemId,
                    Name = n.Name,
                    Description = n.Description,
                    Status = n.Status,
                    Petitions = n.Petitions.Select(a => new PetitionDto()
                    {
                        PetitionDate = a.PetitionDate,
                        PetitionId = a.PetitionId,
                        PetitionTypeId = a.PetitionType.PetitionTypeId,
                        PetitionUserId = a.SimulationUser.SimulationUserId,
                        Status = a.Status,
                        TargetAgendaItemId = a.AgendaItem.AgendaItemId,
                        Text = a.Text
                    }).ToList()
                });
            if (test.Any())
                return await test.ToListAsync();
            return new List<AgendaItemDto>();
        }

        public async Task<List<AgendaItem>> GetAgendaItems(int simulationId)
        {
            var query = _context.AgendaItems.Where(n => n.Simulation.SimulationId == simulationId);
            var hasElements = await query.AnyAsync();
            if (hasElements) return await query.ToListAsync();
            return new List<AgendaItem>();
        }
        

        public List<SimulationHubConnection> UserConnections(int userId)
        {
            return this._context.SimulationHubConnections.Where(n => n.User.SimulationUserId == userId).ToList();
        }

        internal async Task RemoveUser(int simulationId, int userId)
        {
            
            var user = await this._context.SimulationUser.SingleOrDefaultAsync(n => n.SimulationUserId == userId);
            if (user != null)
            {
                var hubs = await this._context.SimulationHubConnections.Where(n => n.User.SimulationUserId == userId).ToListAsync();
                if (hubs.Any())
                {
                    this._context.SimulationHubConnections.RemoveRange(hubs);
                }
                this._context.SimulationUser.Remove(user);
                await this._context.SaveChangesAsync();
            }
        }

        public SimulationUser JoinSimulation(Simulation simulation, string displayName)
        {
            if (simulation == null)
                return null;

            if (simulation.LobbyMode == MUNity.Schema.Simulation.LobbyModes.Closed)
                return null;

            var user = new SimulationUser()
            {
                DisplayName = displayName,
                CanSelectRole = true
            };

            simulation.Users.Add(user);
            this._context.SaveChanges();
            return user;
        }

        public SimulationUser JoinSimulation(int simulationId, string displayName)
        {
            var simulation = this._context.Simulations.Include(n => n.Users).FirstOrDefault();
            return JoinSimulation(simulation, displayName);
        }

        #endregion
        /// <summary>
        /// Refactor methods that use this implementation
        /// only to get things running
        /// </summary>
        /// <returns></returns>
        [Obsolete("Every call to the database should be behind a service shield. Use this method only to get things running fast and then refactor the code!")]
        internal MunityContext GetDatabaseInstance()
        {
            return this._context;
        }

        public SimulationPetitionTemplate LoadSimulationPetitionTemplate(string path, string name)
        {
            var mdl = new SimulationPetitionTemplate();
            mdl.TemplateName = name;
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                IgnoreBlankLines = true
            };
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, config))
            {
                mdl.Entries = csv.GetRecords<PetitionTemplateEntry>().ToList();
            }
            return mdl;
        }

        internal List<SimulationHubConnection> GetHubConnections(int simulationId)
        {
            return this._context.SimulationHubConnections.Where(n => n.User.Simulation.SimulationId == simulationId).ToList();
        }

        internal int RemoveHubConnection(SimulationHubConnection connection)
        {
            this._context.SimulationHubConnections.Remove(connection);
            return this._context.SaveChanges();
        }

        public void ApplyPetitionTemplateToSimulation(SimulationPetitionTemplate template, int simulationId)
        {
            var simulation = _context.Simulations.Include(n => n.PetitionTypes).FirstOrDefault(n => n.SimulationId == simulationId);
            if (simulation == null) return;

            simulation.PetitionTypes.Clear();
            int index = 0;
            // PetitionTypes erstellen
            foreach (var petitionTypeTemplate in template.Entries)
            {     
                // For now we only match the name!
                var mdlInDb = _context.PetitionTypes.FirstOrDefault(n => n.Name == petitionTypeTemplate.Name);
                if (mdlInDb == null)
                {
                    mdlInDb = new PetitionType()
                    {
                        Name = petitionTypeTemplate.Name,
                        Category = petitionTypeTemplate.Category,
                        Description = petitionTypeTemplate.Description,
                        Reference = petitionTypeTemplate.Reference,
                        Ruling = petitionTypeTemplate.Ruling
                    };
                    _context.PetitionTypes.Add(mdlInDb);
                }
                var newLink = new PetitionTypeSimulation()
                {
                    AllowChairs = petitionTypeTemplate.AllowChairs,
                    AllowDelegates = petitionTypeTemplate.AllowDelegates,
                    AllowNgo = petitionTypeTemplate.AllowNgo,
                    AllowSpectator = petitionTypeTemplate.AllowSpectator,
                    OrderIndex = index,
                    PetitionType = mdlInDb,
                    Simulation = simulation
                };
                simulation.PetitionTypes.Add(newLink);

                index++;
            }
            _context.SaveChanges();

        }
    
        public List<PetitionTypeSimulation> GetPetitionTypesOfSimulation(int simulationId)
        {
            return _context.SimulationPetitionTypes.Include(n => n.PetitionType)
                .Where(n => n.Simulation.SimulationId == simulationId).ToList();
        }

        internal SimulationStatus GetCurrentStatus(int simulationId)
        {
            return _context.SimulationStatuses
                .OrderByDescending(n => n.StatusTime)
                .FirstOrDefault(n => n.Simulation.SimulationId == simulationId);
        }
    }
}
