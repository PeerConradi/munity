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
                yield return new SimulationRole("eg", "Ägypten", RoleTypes.Delegate);
                yield return new SimulationRole("et", "Äthiopien", RoleTypes.Delegate);
                yield return new SimulationRole("br", "Brasilien", RoleTypes.Delegate);
                yield return new SimulationRole("cn", "China", RoleTypes.Delegate);
                yield return new SimulationRole("de", "Deutschland", RoleTypes.Delegate);
                yield return new SimulationRole("ec", "Ecuador", RoleTypes.Delegate);
                yield return new SimulationRole("ir", "Islamische Republik Iran", RoleTypes.Delegate);
                yield return new SimulationRole("jp", "Japan", RoleTypes.Delegate);
                yield return new SimulationRole("ke", "Kenia", RoleTypes.Delegate);
                yield return new SimulationRole("kr", "Republik Korea", RoleTypes.Delegate);
                yield return new SimulationRole("cu", "Kuba", RoleTypes.Delegate);
                yield return new SimulationRole("lv", "Lettland", RoleTypes.Delegate);
                yield return new SimulationRole("mg", "Madagaskar", RoleTypes.Delegate);
                yield return new SimulationRole("mx", "Mexiko", RoleTypes.Delegate);
                yield return new SimulationRole("ng", "Nigeria", RoleTypes.Delegate);
                yield return new SimulationRole("ru", "Russische Föderation", RoleTypes.Delegate);
                yield return new SimulationRole("sg", "Singapur", RoleTypes.Delegate);
                yield return new SimulationRole("za", "Südafrika", RoleTypes.Delegate);
                yield return new SimulationRole("tm", "Turkmenistan", RoleTypes.Delegate);
                yield return new SimulationRole("hu", "Ungarn", RoleTypes.Delegate);
                yield return new SimulationRole("gb", "Vereinigtes Königreich", RoleTypes.Delegate);
                yield return new SimulationRole("us", "Vereinigte Staaten", RoleTypes.Delegate);
                yield return new SimulationRole("vn", "Vietnam", RoleTypes.Delegate);

                yield return new SimulationRole("na-08", "Ceres", RoleTypes.Ngo);
                yield return new SimulationRole("na-09", "Ingenieure ohne Grenzen e.V.", RoleTypes.Ngo);

                yield return new SimulationRole("un", "Presse", RoleTypes.Spectator);
                yield return new SimulationRole("un", "Zuschauer", RoleTypes.Spectator);
                yield return new SimulationRole("un", "Ihre Exzellenz die Generalsekretärin", RoleTypes.Spectator);
            }
        }
    }
}