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
                yield return new SimulationRole("", "Ägypten", RoleTypes.Delegate);
                yield return new SimulationRole("", "Äthiopien", RoleTypes.Delegate);
                yield return new SimulationRole("", "Australien", RoleTypes.Delegate);
                yield return new SimulationRole("", "Bangladesch", RoleTypes.Delegate);
                yield return new SimulationRole("", "Botsuana", RoleTypes.Delegate);
                yield return new SimulationRole("", "Brasilien", RoleTypes.Delegate);
                yield return new SimulationRole("", "China", RoleTypes.Delegate);
                yield return new SimulationRole("", "Gabun", RoleTypes.Delegate);
                yield return new SimulationRole("", "Indien", RoleTypes.Delegate);
                yield return new SimulationRole("", "Islamische Republik Iran", RoleTypes.Delegate);
                yield return new SimulationRole("", "Kenia", RoleTypes.Delegate);
                yield return new SimulationRole("", "Kolumbien", RoleTypes.Delegate);
                yield return new SimulationRole("", "Kongo", RoleTypes.Delegate);
                yield return new SimulationRole("", "Lettland", RoleTypes.Delegate);
                yield return new SimulationRole("", "Mali", RoleTypes.Delegate);
                yield return new SimulationRole("", "Nicaragua", RoleTypes.Delegate);
                yield return new SimulationRole("", "Die Niederlande", RoleTypes.Delegate);
                yield return new SimulationRole("", "Pakistan", RoleTypes.Delegate);
                yield return new SimulationRole("", "Panama", RoleTypes.Delegate);
                yield return new SimulationRole("", "Paraguay", RoleTypes.Delegate);
                yield return new SimulationRole("", "Russische Föderation", RoleTypes.Delegate);
                yield return new SimulationRole("", "Sierra Leone", RoleTypes.Delegate);
                yield return new SimulationRole("", "Singapur", RoleTypes.Delegate);
                yield return new SimulationRole("", "Turkmenistan", RoleTypes.Delegate);
                yield return new SimulationRole("", "Ukraine", RoleTypes.Delegate);
                yield return new SimulationRole("", "Vereinigtes Königreich", RoleTypes.Delegate);

                yield return new SimulationRole("", "Transparency International", RoleTypes.Ngo);
                yield return new SimulationRole("", "Weltbankgruppe", RoleTypes.Ngo);


            }
        }
    }
}