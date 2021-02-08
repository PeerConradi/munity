using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MUNity.Schema.Simulation;
using MUNityCore.DataHandlers.EntityFramework;
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
            simulation.Users.Add(ownerUser);
            _context.SaveChanges();
            return ownerUser;
        }

        public SimulationUser CreateUser(Simulation simulation, string displayName)
        {
            var baseUser = new SimulationUser()
            {
                CanCreateRole = false,
                CanEditListOfSpeakers = false,
                CanEditResolution = false,
                CanSelectRole = false,
                DisplayName = displayName,
                Role = null,
                Simulation = simulation
            };
            simulation.Users.Add(baseUser);
            _context.SaveChanges();
            return baseUser;
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
                n.RoleType == SimulationRole.RoleTypes.Chairman);
            if (currentChairmanRole == null)
            {
                currentChairmanRole = new SimulationRole()
                {
                    RoleType = SimulationRole.RoleTypes.Chairman,
                    Iso = "UN"
                };
                simulation.Roles.Add(currentChairmanRole);
            }

            currentChairmanRole.Name = name;
            this._context.SaveChanges();
            return currentChairmanRole;
        }

        public async Task<bool> SetPhase(int simulationId, SimulationEnums.GamePhases phase)
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
                RoleType = SimulationRole.RoleTypes.Delegate,
                Simulation = simulation,
            };
            simulation.Roles.Add(role);
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

        internal Task<bool> IsTokenValidAndUserAdmin(SimulationRequest requestSchema)
        {
            return _context.SimulationUser.AnyAsync(n => 
            n.Simulation.SimulationId == requestSchema.SimulationId && 
            n.Token == requestSchema.Token &&
            n.CanCreateRole);
        }

        internal Task<bool> IsTokenValid(SimulationRequest requestSchema)
        {
            return _context.SimulationUser.AnyAsync(n =>
            n.Simulation.SimulationId == requestSchema.SimulationId &&
            n.Token == requestSchema.Token);
        }

        internal Task<bool> IsTokenValid(int simulationId, string token)
        {
            return _context.SimulationUser.AnyAsync(n =>
            n.Simulation.SimulationId == simulationId &&
            n.Token == token);
        }

        internal Task<bool> IsTokenValidAndUserDelegate(SimulationRequest requestSchema)
        {
            return _context.SimulationUser.AnyAsync(n =>
            n.Simulation.SimulationId == requestSchema.SimulationId &&
            n.Token == requestSchema.Token &&
            n.Role != null &&
            n.Role.RoleType == SimulationRole.RoleTypes.Delegate);
        }

        internal Task<bool> IsTokenValidAndUserChair(SimulationRequest requestSchema)
        {
            return _context.SimulationUser.AnyAsync(n =>
            n.Simulation.SimulationId == requestSchema.SimulationId &&
            n.Token == requestSchema.Token &&
            n.Role != null &&
            n.Role.RoleType == SimulationRole.RoleTypes.Chairman);
        }

        internal Task<bool> IsTokenValidAndUserChair(int simulationId, string token)
        {
            return _context.SimulationUser.AnyAsync(n =>
            n.Simulation.SimulationId == simulationId &&
            n.Token == token &&
            n.Role != null &&
            n.Role.RoleType == SimulationRole.RoleTypes.Chairman);
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

        public Task<List<Models.Simulation.PetitionType>> GetSimulationPetitionTypes(int simulationId)
        {
            return _context.SimulationPetitionTypes.Where(n => n.Simulation.SimulationId == simulationId)
                .Include(n => n.PetitionType).Select(n => n.PetitionType).ToListAsync();
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

            if (simulation.LobbyMode == MUNity.Schema.Simulation.SimulationEnums.LobbyModes.Closed)
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

        public async Task<int> CreateDefaultPetitionTypes()
        {
            var hasElements = await _context.PetitionTypes.AnyAsync();
            if (hasElements) return 0;

            _context.PetitionTypes.Add(new Models.Simulation.PetitionType() { Name = "Recht auf Information", Category = "Persönlicher Antrag", Description = "Für Fragen zur Geschäftsordnung oder zum Verfahren (z.B.zu Anträgen, Einreichen von Arbeitspapieren). Außerdem für Bitten(z.B.Fenster öffnen, Licht einschalten, lauter sprechen).", Reference = "§ 14 Abs. 1 Nr. 1", Ruling = MUNity.Schema.Simulation.PetitionType.PetitionRulings.Chairs });
            _context.PetitionTypes.Add(new Models.Simulation.PetitionType() { Name = "Recht auf Wiederherstellung der Ordnung", Category = "Persönlicher Antrag", Description = "Um Verfahrensfehler oder Verstöße gegen die Geschäftsordnung zur Sprache zu bringen.", Reference = "§ 14 Abs. 1 Nr. 2", Ruling = MUNity.Schema.Simulation.PetitionType.PetitionRulings.Chairs });
            _context.PetitionTypes.Add(new Models.Simulation.PetitionType() { Name = "Recht auf Klärung eines Missverständnisses", Category = "Persönlicher Antrag", Description = "Nur nach einer Erwiderung von dem*der Redner*in auf eine eigene missverstandene und unbeantwortet elassene Frage oder Kurzbemerkung möglich.", Reference = "$ 14 Abs. 1 Nr. 3", Ruling = MUNity.Schema.Simulation.PetitionType.PetitionRulings.Chairs });
            //context.PetitionTypes.Add(new Models.Simulation.PetitionType() {Name = "", Category = "Persönlicher Antrag", Description = "", Reference = "", Ruling = Models.Simulation.PetitionType.PetitionRulings.Chairs });
            await _context.SaveChangesAsync();
            return _context.PetitionTypes.Count();
        }
    }
}
