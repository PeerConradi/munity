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
                yield return new SimulationRole("", "Bangladesch", RoleTypes.Delegate);
                yield return new SimulationRole("", "Burundi", RoleTypes.Delegate);
                yield return new SimulationRole("", "China", RoleTypes.Delegate);
                yield return new SimulationRole("", "Estland", RoleTypes.Delegate);
                yield return new SimulationRole("", "Frankreich", RoleTypes.Delegate);
                yield return new SimulationRole("", "Georgien", RoleTypes.Delegate);
                yield return new SimulationRole("", "Guyana", RoleTypes.Delegate);
                yield return new SimulationRole("", "Haiti", RoleTypes.Delegate);
                yield return new SimulationRole("", "Indonesien", RoleTypes.Delegate);
                yield return new SimulationRole("", "Jemen", RoleTypes.Delegate);
                yield return new SimulationRole("", "Jordanien", RoleTypes.Delegate);
                yield return new SimulationRole("", "Kolumbien", RoleTypes.Delegate);
                yield return new SimulationRole("", "Republik Korea", RoleTypes.Delegate);
                yield return new SimulationRole("", "Mali", RoleTypes.Delegate);
                yield return new SimulationRole("", "Neuseeland", RoleTypes.Delegate);
                yield return new SimulationRole("", "Die Niederlande", RoleTypes.Delegate);
                yield return new SimulationRole("", "Niger", RoleTypes.Delegate);
                yield return new SimulationRole("", "Ruanda", RoleTypes.Delegate);
                yield return new SimulationRole("", "Serbien", RoleTypes.Delegate);
                yield return new SimulationRole("", "St. Vincent und die Grenadinen", RoleTypes.Delegate);
                yield return new SimulationRole("", "Südafrika", RoleTypes.Delegate);
                yield return new SimulationRole("", "Togo", RoleTypes.Delegate);

                yield return new SimulationRole("", "Islamic Relief Worldwide", RoleTypes.Ngo);
                yield return new SimulationRole("", "Kinderhilfswerk der Vereinten Nationen (UNICEF)", RoleTypes.Ngo);

            }
        }
    }
}