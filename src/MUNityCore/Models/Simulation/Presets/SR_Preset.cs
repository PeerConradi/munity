using MUNity.Schema.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.Simulation.Presets
{
    public class SR_Preset : ISimulationPreset
    {
        public string Id => "SR";

        public string Name => "Sicherheitsrat";

        public IEnumerable<SimulationRole> Roles
        {
            get
            {
                yield return new SimulationRole("un", "Vorsitzende(r)", RoleTypes.Chairman);
                yield return new SimulationRole("", "China", RoleTypes.Delegate);
                yield return new SimulationRole("", "Deutschland", RoleTypes.Delegate);
                yield return new SimulationRole("", "Dominikanische Republik", RoleTypes.Delegate);
                yield return new SimulationRole("", "Estland", RoleTypes.Delegate);
                yield return new SimulationRole("", "Frankreich", RoleTypes.Delegate);
                yield return new SimulationRole("", "Indonesien", RoleTypes.Delegate);
                yield return new SimulationRole("", "Niger", RoleTypes.Delegate);
                yield return new SimulationRole("", "Russische Föderation", RoleTypes.Delegate);
                yield return new SimulationRole("", "St. Vincent und die Grenadinen", RoleTypes.Delegate);
                yield return new SimulationRole("", "Südafrika", RoleTypes.Delegate);
                yield return new SimulationRole("", "Tunesien", RoleTypes.Delegate);
                yield return new SimulationRole("", "Vereinigtes Königreich", RoleTypes.Delegate);
                yield return new SimulationRole("", "Vereinigte Staaten", RoleTypes.Delegate);
                yield return new SimulationRole("", "Vietnam", RoleTypes.Delegate);

                yield return new SimulationRole("", "International Crisis Group", RoleTypes.Ngo);
                yield return new SimulationRole("", "Mercy Corps", RoleTypes.Ngo);
            }
        }
    }
}