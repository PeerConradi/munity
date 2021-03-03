using MUNity.Schema.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.Simulation.Presets
{
    public class HA3_Preset : ISimulationPreset
    {
        public string Id => "ha3";

        public string Name => "Hauptausschuss 3";

        public IEnumerable<SimulationRole> Roles
        {
            get
            {
                yield return new SimulationRole("un", "Vorsitzende(r)", RoleTypes.Chairman);
                yield return new SimulationRole("dz", "Algerien", RoleTypes.Delegate);
                yield return new SimulationRole("bd", "Bangladesch", RoleTypes.Delegate);
                yield return new SimulationRole("bi", "Burundi", RoleTypes.Delegate);
                yield return new SimulationRole("cn", "China", RoleTypes.Delegate);
                yield return new SimulationRole("ee", "Estland", RoleTypes.Delegate);
                yield return new SimulationRole("fr", "Frankreich", RoleTypes.Delegate);
                yield return new SimulationRole("ge", "Georgien", RoleTypes.Delegate);
                yield return new SimulationRole("gy", "Guyana", RoleTypes.Delegate);
                yield return new SimulationRole("ht", "Haiti", RoleTypes.Delegate);
                yield return new SimulationRole("id", "Indonesien", RoleTypes.Delegate);
                yield return new SimulationRole("ye", "Jemen", RoleTypes.Delegate);
                yield return new SimulationRole("jo", "Jordanien", RoleTypes.Delegate);
                yield return new SimulationRole("co", "Kolumbien", RoleTypes.Delegate);
                yield return new SimulationRole("kr", "Republik Korea", RoleTypes.Delegate);
                yield return new SimulationRole("ml", "Mali", RoleTypes.Delegate);
                yield return new SimulationRole("nz", "Neuseeland", RoleTypes.Delegate);
                yield return new SimulationRole("nl", "Niederlande", RoleTypes.Delegate);
                yield return new SimulationRole("ne", "Niger", RoleTypes.Delegate);
                yield return new SimulationRole("rw", "Ruanda", RoleTypes.Delegate);
                yield return new SimulationRole("rs", "Serbien", RoleTypes.Delegate);
                yield return new SimulationRole("vc", "St. Vincent und die Grenadinen", RoleTypes.Delegate);
                yield return new SimulationRole("za", "Südafrika", RoleTypes.Delegate);
                yield return new SimulationRole("tg", "Togo", RoleTypes.Delegate);

                yield return new SimulationRole("na-04", "Islamic Relief Worldwide", RoleTypes.Ngo);
                yield return new SimulationRole("na-05", "Kinderhilfswerk der Vereinten Nationen (UNICEF)", RoleTypes.Ngo);

                yield return new SimulationRole("un", "Presse", RoleTypes.Spectator);
                yield return new SimulationRole("un", "Zuschauer", RoleTypes.Spectator);
                yield return new SimulationRole("un", "Ihre Exzellenz die Generalsekretärin", RoleTypes.Spectator);
            }
        }
    }
}