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
                yield return new SimulationRole("ar", "Argentinien", RoleTypes.Delegate);
                yield return new SimulationRole("am", "Armenien", RoleTypes.Delegate);
                yield return new SimulationRole("bd", "Bangladesch", RoleTypes.Delegate);
                yield return new SimulationRole("be", "Belgien", RoleTypes.Delegate);
                yield return new SimulationRole("br", "Brasilien", RoleTypes.Delegate);
                yield return new SimulationRole("dk", "Dänemark", RoleTypes.Delegate);
                yield return new SimulationRole("de", "Deutschland", RoleTypes.Delegate);
                yield return new SimulationRole("fj", "Fidschi", RoleTypes.Delegate);
                yield return new SimulationRole("in", "Indien", RoleTypes.Delegate);
                yield return new SimulationRole("id", "Indonesien", RoleTypes.Delegate);
                yield return new SimulationRole("ki", "Kiribati", RoleTypes.Delegate);
                yield return new SimulationRole("cd", "DR Kongo", RoleTypes.Delegate);
                yield return new SimulationRole("kr", "Republik Korea", RoleTypes.Delegate);
                yield return new SimulationRole("lr", "Liberia", RoleTypes.Delegate);
                yield return new SimulationRole("ly", "Libyen", RoleTypes.Delegate);
                yield return new SimulationRole("nl", "Niederlande", RoleTypes.Delegate);
                yield return new SimulationRole("sl", "Sierra Leone", RoleTypes.Delegate);
                yield return new SimulationRole("sk", "Slowakei", RoleTypes.Delegate);
                yield return new SimulationRole("so", "Somalia", RoleTypes.Delegate);
                yield return new SimulationRole("tg", "Togo", RoleTypes.Delegate);
                yield return new SimulationRole("cz", "Tschechische Republik", RoleTypes.Delegate);
                yield return new SimulationRole("uy", "Uruguay", RoleTypes.Delegate);
                yield return new SimulationRole("ve", "Bolivarische Republik Venezuela", RoleTypes.Delegate);

                yield return new SimulationRole("na-14", "Friends of the Earth", RoleTypes.Ngo);
                yield return new SimulationRole("na-15", "Human Rights Watch", RoleTypes.Ngo);
                // yield return new SimulationRole("", "MRR: BE BR KI VE", RoleTypes.Ngo);//TODO: ?!

                yield return new SimulationRole("un", "Presse", RoleTypes.Spectator);
                yield return new SimulationRole("un", "Zuschauer", RoleTypes.Spectator);
                yield return new SimulationRole("un", "Ihre Exzellenz die Generalsekretärin", RoleTypes.Spectator);
            }
        }
    }
}