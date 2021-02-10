using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.Simulation.Presets
{
    public interface ISimulationPreset
    {
        string Id { get; }

        string Name { get; }

        IEnumerable<SimulationRole> Roles { get; }
    }
}
