using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        

        public Task<Simulation> GetSimulationWithUsersAndRoles(int id)
        {
            return this._context.Simulations
                .Include(n => n.Roles)
                .Include(n => n.Users)
                .FirstOrDefaultAsync(n => n.SimulationId == id);
        }

        public Simulation GetSImulationWithHubsUsersAndRoles(int id)
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

        public IQueryable<SimulationRole> GetSimulationsRoles(int simulationId)
        {
            return this._context.SimulationRoles.Where(n => n.Simulation.SimulationId == simulationId);
        }

        public IQueryable<SimulationUser> GetSimulationUsers(int simulationId)
        {
            return this._context.SimulationUser.Where(n => n.Simulation.SimulationId == simulationId);
        }

        public SimulationUser GetSimulationUser(int simulationId, string token)
        {
            return this._context.SimulationUser.FirstOrDefault(n => n.Simulation.SimulationId == simulationId && n.Token == token);
        }

        public void SaveDbChanges()
        {
            this._context.SaveChanges();
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

        internal void ApplyPreset(Simulation simulation, ISimulationPreset preset, bool removeExistingRoles = true)
        {
            if (removeExistingRoles) simulation.Roles.Clear();
            simulation.Roles.AddRange(preset.Roles);
            this._context.SaveChanges();
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

        public bool BecomeRole(Simulation simulation, SimulationUser user, SimulationRole role)
        {
            var users = this._context.SimulationUser
                .Include(n => n.Role)
                .Where(n => n.Simulation.SimulationId == simulation.SimulationId);

            // Cannot take this role because it is already taken!
            if (users.Any(n => n.Role.SimulationRoleId == role.SimulationRoleId)) return false;

            user.Role = role;
            this._context.SaveChanges();
            return true;
        }

        public SimulationService(MunityContext context)
        {
            this._context = context;
        }

    }
}
