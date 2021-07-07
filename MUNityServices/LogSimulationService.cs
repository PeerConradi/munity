using Microsoft.EntityFrameworkCore;
using MUNity.Schema.Simulation;
using MUNityCore.DataHandlers.EntityFramework;
using MUNityCore.Models.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Services
{
    public class LogSimulationService
    {
        private MunityContext _context;

        public const string TYPE_USERS = "USERS";

        public const string TYPE_AGENDA = "AGENDA";

        public const string TYPE_VOTING = "VOTE";

        public const string TYPE_STATUS = "STATUS";

        public LogSimulationService(MunityContext context)
        {
            _context = context;
        }

        internal void LogForSimulation(int simulationId, string category, string name, string text)
        {
            var simulation = this._context.Simulations.FirstOrDefault(n => n.SimulationId == simulationId);
            var logEntry = new SimulationLog()
            {
                CategoryName = category,
                Name = name,
                Simulation = simulation,
                Text = text,
                Timestamp = DateTime.Now
            };
            this._context.SimulationLog.Add(logEntry);
            this._context.SaveChanges();
        }

        internal void LogUserConnected(int userId)
        {
            var user = _context.SimulationUser.Include(n => n.Simulation).FirstOrDefault(n => n.SimulationUserId == userId);
            if (user != null && user.Simulation != null)
            {
                var logEntry = new SimulationLog()
                {
                    CategoryName = TYPE_USERS,
                    Name = "User Connected",
                    Simulation = user.Simulation,
                    Text = $"User Connected {user.DisplayName} ({user.SimulationUserId})",
                    Timestamp = DateTime.Now
                };
                this._context.SimulationLog.Add(logEntry);
                this._context.SaveChanges();
            }
        }

        internal void LogUserDisconnected(int userId)
        {
            var user = _context.SimulationUser.Include(n => n.Simulation).FirstOrDefault(n => n.SimulationUserId == userId);
            if (user != null && user.Simulation != null)
            {
                var logEntry = new SimulationLog()
                {
                    CategoryName = TYPE_USERS,
                    Name = "User Connected",
                    Simulation = user.Simulation,
                    Text = $"User Connected {user.DisplayName} ({user.SimulationUserId})",
                    Timestamp = DateTime.Now
                };
                this._context.SimulationLog.Add(logEntry);
                this._context.SaveChanges();
            }
        }

        internal void LogVotingCreated(int simulationId, string text)
        {
            LogForSimulation(simulationId, TYPE_VOTING, "Abstimmung erstellt", $"Abstimmung über \"{text}\" erstellt.");
        }

        internal void LogAgendaItemCreated(int simulationId, string name)
        {
            LogForSimulation(simulationId, TYPE_AGENDA, "Tagesordnungspunkt erstellt", $"Neuen Tagesordnungspunkt \"{name}\" erstellt.");
        }

        internal void LogAgendaItemRemoved(int simulationId, string name)
        {
            LogForSimulation(simulationId, TYPE_AGENDA, "Tagesordnungspunkt gelöscht", $"Der Tagesordnungspunkt \"{name}\" wurde entfernt.");
        }

        internal void LogPetitionCreated(int simulationId, PetitionInfoDto info)
        {
            var name = this._context.AgendaItems.FirstOrDefault(n => n.AgendaItemId == info.AgendaItemId)?.Name;
            LogForSimulation(simulationId, TYPE_AGENDA, "Antrag gestellt", $"Neuer Antrag {info.TypeName} durch {info.SubmitterRoleName} in {name}");
        }

        public List<SimulationLog> GetLogOfSimulation(int simulationId)
        {
            return this._context.SimulationLog.Where(n => n.Simulation.SimulationId == simulationId).OrderBy(n => n.Timestamp).ToList();
        }

        internal List<SimulationLog> GetFilteredLogOfSimulation(int simulationId, bool userLog, bool agendaLog, bool votingLog, bool statusLog)
        {
            var log = this._context.SimulationLog.Where(n => n.Simulation.SimulationId == simulationId &&
                (userLog && n.CategoryName == TYPE_USERS) ||
                (agendaLog && n.CategoryName == TYPE_AGENDA) ||
                (votingLog && n.CategoryName == TYPE_VOTING) ||
                (statusLog && n.CategoryName == TYPE_STATUS)).OrderByDescending(n => n.Timestamp);

            return log.ToList();
        }
    }
}
