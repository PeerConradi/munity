using MUNity.Schema.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.Simulation.Presets
{
    public class PresetSicherheitsrat : ISimulationPreset
    {
        public string Id => "de_sc";

        public string Name => "UN Sicherheitsrat";

        public IEnumerable<SimulationRole> Roles
        {
            get
            {
                yield return new SimulationRole("un", "Vorsitzende(r)", RoleTypes.Chairman);
                yield return new SimulationRole("fr", "Frankreich", RoleTypes.Delegate);
                yield return new SimulationRole("ru", "Russland", RoleTypes.Delegate);
                yield return new SimulationRole("gb", "Vereinigtes Königreich", RoleTypes.Delegate);
                yield return new SimulationRole("us", "Vereinigte Staaten", RoleTypes.Delegate);
                yield return new SimulationRole("cn", "Volksrepublik China", RoleTypes.Delegate);
                yield return new SimulationRole("be", "Belgien", RoleTypes.Delegate);
                yield return new SimulationRole("de", "Deutschland", RoleTypes.Delegate);
                yield return new SimulationRole("do", "Dominikanische Republik", RoleTypes.Delegate);
                yield return new SimulationRole("id", "Republik Indonesien", RoleTypes.Delegate);
                yield return new SimulationRole("za", "Republik Südafrika", RoleTypes.Delegate);
                yield return new SimulationRole("ee", "Estland", RoleTypes.Delegate);
                yield return new SimulationRole("ne", "Niger", RoleTypes.Delegate);
                yield return new SimulationRole("vc", "St. Vincent und die Grenadinen", RoleTypes.Delegate);
                yield return new SimulationRole("tn", "Tunesien", RoleTypes.Delegate);
                yield return new SimulationRole("vn", "Vietnam", RoleTypes.Delegate);
            }
        }
    }
}
