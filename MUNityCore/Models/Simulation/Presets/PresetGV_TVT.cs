using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.Simulation.Presets
{
    public class PresetGV_TVT : ISimulationPreset
    {
        public string Id => "tvt_gv";

        public string Name => "TVT Generalversammlung";

        public IEnumerable<SimulationRole> Roles
        {
            get
            {
                yield return new SimulationRole("un", "Vorsitzende(r)", SimulationRole.RoleTypes.Chairman);
                yield return new SimulationRole("af", "Afghanistan", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("dz", "Algerien", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("ar", "Argentinien", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("bh", "Bahrain", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("be", "Belgien", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("br", "Brasilien", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("cn", "China", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("dj", "Dschibuti", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("fr", "Frankreich", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("id", "Indonesien", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("it", "Italien", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("jp", "Japan", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("ke", "Kenia", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("co", "Kolumbien", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("ml", "Mali", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("mm", "Myanmar", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("no", "Norwegen", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("at", "Österreich", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("pk", "Pakistan", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("ph", "Philippinen ", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("ru", "Russland", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("sa", "Saudi-Arabien", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("sg", "Singapur", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("gb", "Vereinigtes Königreich", SimulationRole.RoleTypes.Delegate);
                yield return new SimulationRole("us", "Vereinigte Staaten", SimulationRole.RoleTypes.Delegate);

                yield return new SimulationRole("n0", "Rotes Kreuz", SimulationRole.RoleTypes.Ngo);
                yield return new SimulationRole("n1", "Save the Children", SimulationRole.RoleTypes.Ngo);


            }
        }
    }
}
