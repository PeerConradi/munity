using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityCore.Models.Conference;
using MUNityCore.Models.Simulation;
using MUNityCore.Schema.Response.Simulation;

namespace MUNityCore.Hubs
{
    public interface ITypedSimulationHub
    {
        Task RolesChanged(int simulationId, IEnumerable<SimulationRoleItem> Roles);

        Task UserRoleChanged(int simulationId, int userId, int roleId);

        Task UserConnected(int simulationId, SimulationUserItem user);

        Task UserDisconnected(int simulationId, SimulationUserItem user);

        Task PhaseChanged(int simulationId, Simulation.GamePhases phase);

        Task StatusChanged(int simulationId, string newStatus);

        Task LobbyModeChanged(int simulationId, Simulation.LobbyModes mode);
    }
}
