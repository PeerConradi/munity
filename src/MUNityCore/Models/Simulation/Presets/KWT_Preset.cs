using MUNity.Schema.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.Simulation.Presets
{
    public class KWT_Preset : ISimulationPreset
    {
        public string Id => "KWT";

        public string Name => "Kommission für Wissenschaft und Technik";

        public IEnumerable<SimulationRole> Roles
        {
            get
            {
                yield return new SimulationRole("un", "Vorsitzende(r)", RoleTypes.Chairman);
                yield return new SimulationRole("", "Ägypten", RoleTypes.Delegate);
                yield return new SimulationRole("", "Äthiopien", RoleTypes.Delegate);
                yield return new SimulationRole("", "Brasilien", RoleTypes.Delegate);
                yield return new SimulationRole("", "China", RoleTypes.Delegate);
                yield return new SimulationRole("", "Deutschland", RoleTypes.Delegate);
                yield return new SimulationRole("", "Ecuador", RoleTypes.Delegate);
                yield return new SimulationRole("", "Islamische Republik Iran", RoleTypes.Delegate);
                yield return new SimulationRole("", "Japan", RoleTypes.Delegate);
                yield return new SimulationRole("", "Kenia", RoleTypes.Delegate);
                yield return new SimulationRole("", "Republik Korea", RoleTypes.Delegate);
                yield return new SimulationRole("", "Kuba", RoleTypes.Delegate);
                yield return new SimulationRole("", "Lettland", RoleTypes.Delegate);
                yield return new SimulationRole("", "Madagaskar", RoleTypes.Delegate);
                yield return new SimulationRole("", "Mexiko", RoleTypes.Delegate);
                yield return new SimulationRole("", "Nigeria", RoleTypes.Delegate);
                yield return new SimulationRole("", "Russische Föderation", RoleTypes.Delegate);
                yield return new SimulationRole("", "Singapur", RoleTypes.Delegate);
                yield return new SimulationRole("", "Südafrika", RoleTypes.Delegate);
                yield return new SimulationRole("", "Turkmenistan", RoleTypes.Delegate);
                yield return new SimulationRole("", "Ungarn", RoleTypes.Delegate);
                yield return new SimulationRole("", "Vereinigtes Königreich", RoleTypes.Delegate);
                yield return new SimulationRole("", "Vereinigte Staaten", RoleTypes.Delegate);
                yield return new SimulationRole("", "Vietnam", RoleTypes.Delegate);

                yield return new SimulationRole("", "Ceres", RoleTypes.Ngo);
                yield return new SimulationRole("", "Ingenieure ohne Grenzen e.V.", RoleTypes.Ngo);
            }
        }
    }
}