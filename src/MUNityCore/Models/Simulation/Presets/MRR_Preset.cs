using MUNity.Schema.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.Simulation.Presets
{
    public class MRR_Preset : ISimulationPreset
    {
        public string Id => "MRR";

        public string Name => "Menschenrechtsrat";

        public IEnumerable<SimulationRole> Roles
        {
            get
            {
                yield return new SimulationRole("un", "Vorsitzende(r)", RoleTypes.Chairman);
                yield return new SimulationRole("", "Argentinien", RoleTypes.Delegate);
                yield return new SimulationRole("", "Armenien", RoleTypes.Delegate);
                yield return new SimulationRole("", "Bangladesch", RoleTypes.Delegate);
                yield return new SimulationRole("", "Belgien", RoleTypes.Delegate);
                yield return new SimulationRole("", "Brasilien", RoleTypes.Delegate);
                yield return new SimulationRole("", "Dänemark", RoleTypes.Delegate);
                yield return new SimulationRole("", "Deutschland", RoleTypes.Delegate);
                yield return new SimulationRole("", "Fidschi", RoleTypes.Delegate);
                yield return new SimulationRole("", "Indien", RoleTypes.Delegate);
                yield return new SimulationRole("", "Indonesien", RoleTypes.Delegate);
                yield return new SimulationRole("", "Kiribati", RoleTypes.Delegate);
                yield return new SimulationRole("", "DR Kongo", RoleTypes.Delegate);
                yield return new SimulationRole("", "Republik Korea", RoleTypes.Delegate);
                yield return new SimulationRole("", "Liberia", RoleTypes.Delegate);
                yield return new SimulationRole("", "Libyen", RoleTypes.Delegate);
                yield return new SimulationRole("", "Die Niederlande", RoleTypes.Delegate);
                yield return new SimulationRole("", "Sierra Leone", RoleTypes.Delegate);
                yield return new SimulationRole("", "Slowakei", RoleTypes.Delegate);
                yield return new SimulationRole("", "Somalia", RoleTypes.Delegate);
                yield return new SimulationRole("", "Togo", RoleTypes.Delegate);
                yield return new SimulationRole("", "Tschechische Republik", RoleTypes.Delegate);
                yield return new SimulationRole("", "Uruguay", RoleTypes.Delegate);
                yield return new SimulationRole("", "Bolivarische Republik Venezuela", RoleTypes.Delegate);

                yield return new SimulationRole("", "Friends of the Earth", RoleTypes.Ngo);
                yield return new SimulationRole("", "Human Rights Watch", RoleTypes.Ngo);
                yield return new SimulationRole("", "MRR: BE BR KI VE", RoleTypes.Ngo);//TODO: ?!
            }
        }
    }
}