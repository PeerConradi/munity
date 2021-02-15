using MUNity.Schema.Simulation;
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
                yield return new SimulationRole("un", "Vorsitzende(r)", RoleTypes.Chairman);
                yield return new SimulationRole("af", "Afghanistan", RoleTypes.Delegate);
                yield return new SimulationRole("dz", "Algerien", RoleTypes.Delegate);
                yield return new SimulationRole("ar", "Argentinien", RoleTypes.Delegate);
                yield return new SimulationRole("bh", "Bahrain", RoleTypes.Delegate);
                yield return new SimulationRole("be", "Belgien", RoleTypes.Delegate);
                yield return new SimulationRole("br", "Brasilien", RoleTypes.Delegate);
                yield return new SimulationRole("cn", "China", RoleTypes.Delegate);
                yield return new SimulationRole("dj", "Dschibuti", RoleTypes.Delegate);
                yield return new SimulationRole("fr", "Frankreich", RoleTypes.Delegate);
                yield return new SimulationRole("id", "Indonesien", RoleTypes.Delegate);
                yield return new SimulationRole("it", "Italien", RoleTypes.Delegate);
                yield return new SimulationRole("jp", "Japan", RoleTypes.Delegate);
                yield return new SimulationRole("ke", "Kenia", RoleTypes.Delegate);
                yield return new SimulationRole("co", "Kolumbien", RoleTypes.Delegate);
                yield return new SimulationRole("ml", "Mali", RoleTypes.Delegate);
                yield return new SimulationRole("mm", "Myanmar", RoleTypes.Delegate);
                yield return new SimulationRole("no", "Norwegen", RoleTypes.Delegate);
                yield return new SimulationRole("at", "Österreich", RoleTypes.Delegate);
                yield return new SimulationRole("pk", "Pakistan", RoleTypes.Delegate);
                yield return new SimulationRole("ph", "Philippinen ", RoleTypes.Delegate);
                yield return new SimulationRole("ru", "Russland", RoleTypes.Delegate);
                yield return new SimulationRole("sa", "Saudi-Arabien", RoleTypes.Delegate);
                yield return new SimulationRole("sg", "Singapur", RoleTypes.Delegate);
                yield return new SimulationRole("gb", "Vereinigtes Königreich", RoleTypes.Delegate);
                yield return new SimulationRole("us", "Vereinigte Staaten", RoleTypes.Delegate);

                yield return new SimulationRole("n0", "Rotes Kreuz", RoleTypes.Ngo);
                yield return new SimulationRole("n1", "Save the Children", RoleTypes.Ngo);


            }
        }
    }
}
