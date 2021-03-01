using MUNity.Schema.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.Simulation.Presets
{
    public class WiSo_Preset : ISimulationPreset
    {
        public string Id => "WiSo";

        public string Name => "Wirtschafts- und Sozialrat";

        public IEnumerable<SimulationRole> Roles
        {
            get
            {
                yield return new SimulationRole("un", "Vorsitzende(r)", RoleTypes.Chairman);
                yield return new SimulationRole("eg", "Ägypten", RoleTypes.Delegate);
                yield return new SimulationRole("et", "Äthiopien", RoleTypes.Delegate);
                yield return new SimulationRole("au", "Australien", RoleTypes.Delegate);
                yield return new SimulationRole("bd", "Bangladesch", RoleTypes.Delegate);
                yield return new SimulationRole("bw", "Botsuana", RoleTypes.Delegate);
                yield return new SimulationRole("br", "Brasilien", RoleTypes.Delegate);
                yield return new SimulationRole("cn", "China", RoleTypes.Delegate);
                yield return new SimulationRole("ga", "Gabun", RoleTypes.Delegate);
                yield return new SimulationRole("in", "Indien", RoleTypes.Delegate);
                yield return new SimulationRole("ir", "Islamische Republik Iran", RoleTypes.Delegate);
                yield return new SimulationRole("ke", "Kenia", RoleTypes.Delegate);
                yield return new SimulationRole("co", "Kolumbien", RoleTypes.Delegate);
                yield return new SimulationRole("cg", "Republik Kongo", RoleTypes.Delegate);
                yield return new SimulationRole("lv", "Lettland", RoleTypes.Delegate);
                yield return new SimulationRole("ml", "Mali", RoleTypes.Delegate);
                yield return new SimulationRole("ni", "Nicaragua", RoleTypes.Delegate);
                yield return new SimulationRole("nl", "Niederlande", RoleTypes.Delegate);
                yield return new SimulationRole("pk", "Pakistan", RoleTypes.Delegate);
                yield return new SimulationRole("pa", "Panama", RoleTypes.Delegate);
                yield return new SimulationRole("py", "Paraguay", RoleTypes.Delegate);
                yield return new SimulationRole("ru", "Russische Föderation", RoleTypes.Delegate);
                yield return new SimulationRole("sl", "Sierra Leone", RoleTypes.Delegate);
                yield return new SimulationRole("sg", "Singapur", RoleTypes.Delegate);
                yield return new SimulationRole("tm", "Turkmenistan", RoleTypes.Delegate);
                yield return new SimulationRole("ua", "Ukraine", RoleTypes.Delegate);
                yield return new SimulationRole("gb", "Vereinigtes Königreich", RoleTypes.Delegate);
                yield return new SimulationRole("us", "Vereinigte Staaten", RoleTypes.Delegate);

                yield return new SimulationRole("na-06", "Transparency International", RoleTypes.Ngo);
                yield return new SimulationRole("na-07", "Weltbankgruppe", RoleTypes.Ngo);


            }
        }
    }
}