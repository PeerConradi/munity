using MUNity.Schema.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.Simulation.Presets
{
    public class KFK_Preset : ISimulationPreset
    {
        public string Id => "KFK";

        public string Name => "Kommission für Friedenskonsolidierung";

        public IEnumerable<SimulationRole> Roles
        {
            get
            {
                yield return new SimulationRole("un", "Vorsitzende(r)", RoleTypes.Chairman);
                yield return new SimulationRole("", "Äthiopien", RoleTypes.Delegate);
                yield return new SimulationRole("", "Bangladesch", RoleTypes.Delegate);
                yield return new SimulationRole("", "Burundi", RoleTypes.Delegate);
                yield return new SimulationRole("", "Deutschland", RoleTypes.Delegate);
                yield return new SimulationRole("", "Frankreich", RoleTypes.Delegate);
                yield return new SimulationRole("", "Guyana", RoleTypes.Delegate);
                yield return new SimulationRole("", "Haiti", RoleTypes.Delegate);
                yield return new SimulationRole("", "Japan", RoleTypes.Delegate);
                yield return new SimulationRole("", "Mexiko", RoleTypes.Delegate);
                yield return new SimulationRole("", "Nigeria", RoleTypes.Delegate);
                yield return new SimulationRole("", "Oman", RoleTypes.Delegate);
                yield return new SimulationRole("", "Pakistan", RoleTypes.Delegate);
                yield return new SimulationRole("", "Ruanda", RoleTypes.Delegate);
                yield return new SimulationRole("", "Rumänien", RoleTypes.Delegate);
                yield return new SimulationRole("", "Russische Föderation", RoleTypes.Delegate);
                yield return new SimulationRole("", "Schweden", RoleTypes.Delegate);
                yield return new SimulationRole("", "Serbien", RoleTypes.Delegate);
                yield return new SimulationRole("", "Sierra Leone", RoleTypes.Delegate);
                yield return new SimulationRole("", "Sri Lanka", RoleTypes.Delegate);
                yield return new SimulationRole("", "Uruguay", RoleTypes.Delegate);
                yield return new SimulationRole("", "Vereinigte Arabische Emirate", RoleTypes.Delegate);
                yield return new SimulationRole("", "Vereinigte Staaten", RoleTypes.Delegate);

                yield return new SimulationRole("", "GNWP", RoleTypes.Ngo);
                yield return new SimulationRole("", "Intern. Komitee v. Rotem Kreuz", RoleTypes.Ngo);
            }
        }
    }
}