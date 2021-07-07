using MUNityCore.DataHandlers.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityCore.Models.Simulation;
using Microsoft.EntityFrameworkCore;

namespace MUNityCore.Services
{
    public class PresentsService
    {
        private readonly MunityContext _context;

        internal SimulationPresents CreatePresentsCheck(int simulationId)
        {
            var simulation = _context.Simulations.Include(n =>
                n.Users).ThenInclude(n => n.Role).FirstOrDefault(n => n.SimulationId == simulationId);

            if (simulation == null)
                return null;

            var newCheck = new SimulationPresents()
            {
                CheckedDate = null,
                CreatedTime = DateTime.Now,
                MarkedFinished = false,
                Simulation = simulation
            };

            foreach(var user in simulation.Users.Where(n => n.Role != null &&
            (n.Role.RoleType == MUNity.Schema.Simulation.RoleTypes.Delegate || n.Role.RoleType == MUNity.Schema.Simulation.RoleTypes.Ngo)))
            {
                if (user.Role != null && user.Role.IsDelegateOrNgo())
                {
                    var newState = new PresentsState()
                    {
                        RegistertTimestamp = null,
                        SimulationPresents = newCheck,
                        SimulationUser = user,
                        State = PresentsState.PresentsStates.NotChecked,
                        StateValue = ""
                    };
                    _context.PresentStates.Add(newState);
                }
                
            }

            _context.PresentChecks.Add(newCheck);
            _context.SaveChanges();
            return newCheck;
        }

        internal void MarkPresent(int presentsStateId)
        {
            var presentsState = _context.PresentStates.FirstOrDefault(n => n.PresentsStateId == presentsStateId);
            if (presentsState != null)
            {
                presentsState.State = PresentsState.PresentsStates.Present;
                presentsState.RegistertTimestamp = DateTime.Now;
                _context.SaveChanges();
            }
        }

        internal void MarkAbsent(int presentsStateId)
        {
            var presentsState = _context.PresentStates.FirstOrDefault(n => n.PresentsStateId == presentsStateId);
            if (presentsState != null)
            {
                presentsState.State = PresentsState.PresentsStates.Absent;
                presentsState.RegistertTimestamp = DateTime.Now;
                _context.SaveChanges();
            }
        }

        internal SimulationPresents GetLastPresentCheckOfSimulation(int simulationId)
        {
            return _context.PresentChecks
                .Include(n => n.Simulation)
                .Include(n => n.CheckedUsers)
                .ThenInclude(n => n.SimulationUser)
                .ThenInclude(n => n.Role)
                .OrderByDescending(n => n.CreatedTime).FirstOrDefault(n => n.Simulation.SimulationId == simulationId);
        }

        internal void FinishCheck(int checkId)
        {
            var check = _context.PresentChecks.FirstOrDefault(n => n.SimulationPresentsId == checkId);
            if (check != null)
            {
                check.MarkedFinished = true;
            }
            _context.SaveChanges();
        }

        public PresentsService(MunityContext context)
        {
            _context = context;
        }
    }
}
