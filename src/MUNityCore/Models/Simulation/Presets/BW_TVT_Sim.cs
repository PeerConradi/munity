using MUNity.Schema.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.Simulation.Presets
{
    public class BW_TVT_Sim_Preset : ISimulationPreset
    {
        public string Id => "bw_tvt_sim";

        public string Name => "MUNBW TVT Sim";

        public IEnumerable<SimulationRole> Roles
        {
            get
            {
                yield return new SimulationRole("un", "Vorsitzende(r)", RoleTypes.Chairman);
                yield return new SimulationRole("af", "Afghanistan", RoleTypes.Delegate);
                yield return new SimulationRole("cn", "China", RoleTypes.Delegate);
                yield return new SimulationRole("fr", "Frankreich", RoleTypes.Delegate);
                yield return new SimulationRole("id", "Indonesien", RoleTypes.Delegate);
                yield return new SimulationRole("ke", "Kenia", RoleTypes.Delegate);
                yield return new SimulationRole("co", "Kolumbien", RoleTypes.Delegate);
                yield return new SimulationRole("ml", "Mali", RoleTypes.Delegate);
                yield return new SimulationRole("no", "Norwegen", RoleTypes.Delegate);
                yield return new SimulationRole("pk", "Pakistan", RoleTypes.Delegate);
                yield return new SimulationRole("us", "Vereinigte Staaten", RoleTypes.Delegate);
            }
        }
    }
}