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
                yield return new SimulationRole("un", "Vorsitzende(r)", SimulationRole.RoleTypes.Chairman);
                yield return new SimulationRole("fr", "Frankreich", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("ru", "Russland", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("gb", "Vereinigtes Königreich", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("us", "Vereinigte Staaten", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("cn", "Volksrepublik China", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("be", "Belgien", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("de", "Deutschland", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("do", "Dominikanische Republik", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("id", "Republik Indonesien", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("za", "Republik Südafrika", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("ee", "Estland", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("ne", "Niger", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("vc", "St. Vincent und die Grenadinen", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("tn", "Tunesien", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("vn", "Vietnam", SimulationRole.RoleTypes.Delegate);
            }
        }
    }
}
