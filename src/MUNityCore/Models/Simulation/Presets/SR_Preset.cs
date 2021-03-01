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
                yield return new SimulationRole("cn", "China", RoleTypes.Delegate);
                yield return new SimulationRole("de", "Deutschland", RoleTypes.Delegate);
                yield return new SimulationRole("do", "Dominikanische Republik", RoleTypes.Delegate);
                yield return new SimulationRole("ee", "Estland", RoleTypes.Delegate);
                yield return new SimulationRole("fr", "Frankreich", RoleTypes.Delegate);
                yield return new SimulationRole("id", "Indonesien", RoleTypes.Delegate);
                yield return new SimulationRole("ne", "Niger", RoleTypes.Delegate);
                yield return new SimulationRole("ru", "Russische Föderation", RoleTypes.Delegate);
                yield return new SimulationRole("vc", "St. Vincent und die Grenadinen", RoleTypes.Delegate);
                yield return new SimulationRole("za", "Südafrika", RoleTypes.Delegate);
                yield return new SimulationRole("tn", "Tunesien", RoleTypes.Delegate);
                yield return new SimulationRole("gb", "Vereinigtes Königreich", RoleTypes.Delegate);
                yield return new SimulationRole("us", "Vereinigte Staaten", RoleTypes.Delegate);
                yield return new SimulationRole("vn", "Vietnam", RoleTypes.Delegate);

                yield return new SimulationRole("na-10", "International Crisis Group", RoleTypes.Ngo);
                yield return new SimulationRole("na-11", "Mercy Corps", RoleTypes.Ngo);
            }
        }
    }
}