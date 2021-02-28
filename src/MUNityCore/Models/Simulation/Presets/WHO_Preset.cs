using MUNity.Schema.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.Simulation.Presets
{
    public class WHO_Preset : ISimulationPreset
    {
        public string Id => "WHO";

        public string Name => "WHO";//TODO

        public IEnumerable<SimulationRole> Roles
        {
            get
            {
                yield return new SimulationRole("un", "Vorsitzende(r)", RoleTypes.Chairman);
                yield return new SimulationRole("", "Brasilien", RoleTypes.Delegate);
                yield return new SimulationRole("", "China", RoleTypes.Delegate);
                yield return new SimulationRole("", "Kambodscha", RoleTypes.Delegate);
                yield return new SimulationRole("", "DR Kongo", RoleTypes.Delegate);
                yield return new SimulationRole("", "Lettland", RoleTypes.Delegate);
                yield return new SimulationRole("", "Marokko", RoleTypes.Delegate);
                yield return new SimulationRole("", "Neuseeland", RoleTypes.Delegate);
                yield return new SimulationRole("", "Nigeria", RoleTypes.Delegate);
                yield return new SimulationRole("", "Papua-Neuguinea", RoleTypes.Delegate);
                yield return new SimulationRole("", "Peru", RoleTypes.Delegate);
                yield return new SimulationRole("", "Rumänien", RoleTypes.Delegate);
                yield return new SimulationRole("", "Tunesien", RoleTypes.Delegate);
                yield return new SimulationRole("", "Vereinigtes Königreich", RoleTypes.Delegate);

                yield return new SimulationRole("", "Bill & Melinda Gates Foundation", RoleTypes.Ngo);
                yield return new SimulationRole("", "Plan International", RoleTypes.Ngo);

            }
        }
    }
}