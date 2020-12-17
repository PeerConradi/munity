﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MUNityCore.DataHandlers.EntityFramework;
using MUNityCore.Models.Simulation;
using MUNityCore.Schema.Response.Simulation;

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

        public Simulation CreateSimulation(string name, string password, string adminName)
        {
            var sim = new Simulation()
            {
                Name = name,
                Password = password,
                SimulationId = new Random().Next(100000, 999999)
            };

            CreateAndAssignOwner(sim, adminName);

            this._context.Simulations.Add(sim);
            this._context.SaveChanges();
            return sim;
        }

        private void CreateAndAssignOwner(Simulation simulation, string displayName)
        {
            SimulationRole ownerRole = new SimulationRole()
            {
                Name = "Owner",
                Iso = "UN",
                RoleKey = Util.Tools.IdGenerator.RandomString(32),
                RoleType = SimulationRole.RoleTypes.Moderator,
                RoleMaxSlots = 1,
            };

            simulation.Roles.Add(ownerRole);
            var ownerUser = new SimulationUser()
            {
                CanCreateRole = true,
                CanEditListOfSpeakers = true,
                CanEditResolution = true,
                CanSelectRole = true,
                DisplayName = displayName,
                Role = ownerRole,
                Simulation = simulation,
            };
        }


        public Task<Simulation> GetSimulation(int id)
        {
            return this._context.Simulations.FirstOrDefaultAsync(n => n.SimulationId == id);
        }

        public IQueryable<SimulationRole> GetSimulationsRoles(int simulationId)
        {
            return this._context.SimulationRoles.Where(n => n.Simulation.SimulationId == simulationId);
        }

        public IQueryable<SimulationUser> GetSimulationUsers(int simulationId)
        {
            return this._context.SimulationUser.Where(n => n.Simulation.SimulationId == simulationId);
        }

        public IQueryable<SimulationResponses.SimulationList> GetSimulationFront()
        {
            return _context.Simulations
                .Where(n => n.CanJoin)
                .Select(n => new SimulationResponses.SimulationList()
                {
                    Name = n.Name,
                    UsingPassword = n.UsingPassword,
                    SimulationId = n.SimulationId
                });
        }

        public SimulationRole AddChairmanRole(int simulationid, int slotCount, string name)
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
                    Iso = "UN",
                    RoleKey = Util.Tools.IdGenerator.RandomString(32),
                };
                simulation.Roles.Add(currentChairmanRole);
            }

            currentChairmanRole.RoleMaxSlots = slotCount;
            currentChairmanRole.Name = name;
            this._context.SaveChanges();
            return currentChairmanRole;
        }

        public SimulationUser JoinSimulation(Simulation simulation, string displayName)
        {
            if (simulation == null)
                return null;

            if (simulation.LobbyMode == Simulation.LobbyModes.Closed)
                return null;

            var user = new SimulationUser()
            {
                DisplayName = displayName
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

            // You cannot become this role because there are too many users currently in this position.
            if (users.Any())
            {
                if (users.Count(n => n.Role.SimulationRoleId == role.SimulationRoleId) >= role.RoleMaxSlots)
                    return false;
            }

            if (role.RoleMaxSlots == 0)
                return false;

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