using MUNity.Schema.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.Simulation.Presets
{
    public class BW_TVT_EGO : ISimulationPreset
    {
        public string Id => "bw_tvt_ego";

        public string Name => "MUNBW TVT Ego";

        public IEnumerable<SimulationRole> Roles
        {
            get
            {
                yield return new SimulationRole("un", "Vorsitzende(r)", RoleTypes.Chairman);
                yield return new SimulationRole("ao", "Angola", RoleTypes.Delegate);
                yield return new SimulationRole("de", "Deutschland", RoleTypes.Delegate);
                yield return new SimulationRole("cn", "China", RoleTypes.Delegate);
                yield return new SimulationRole("mn", "Mongolei", RoleTypes.Delegate);
                yield return new SimulationRole("us", "Vereinigte Staaten", RoleTypes.Delegate);
                yield return new SimulationRole("na-02", "Greenpeace International", RoleTypes.Ngo);
            }
        }
    }
}