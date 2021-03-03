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
                yield return new SimulationRole("et", "Äthiopien", RoleTypes.Delegate);
                yield return new SimulationRole("bd", "Bangladesch", RoleTypes.Delegate);
                yield return new SimulationRole("bi", "Burundi", RoleTypes.Delegate);
                yield return new SimulationRole("de", "Deutschland", RoleTypes.Delegate);
                yield return new SimulationRole("fr", "Frankreich", RoleTypes.Delegate);
                yield return new SimulationRole("gy", "Guyana", RoleTypes.Delegate);
                yield return new SimulationRole("ht", "Haiti", RoleTypes.Delegate);
                yield return new SimulationRole("jp", "Japan", RoleTypes.Delegate);
                yield return new SimulationRole("mx", "Mexiko", RoleTypes.Delegate);
                yield return new SimulationRole("ng", "Nigeria", RoleTypes.Delegate);
                yield return new SimulationRole("om", "Oman", RoleTypes.Delegate);
                yield return new SimulationRole("pk", "Pakistan", RoleTypes.Delegate);
                yield return new SimulationRole("rw", "Ruanda", RoleTypes.Delegate);
                yield return new SimulationRole("ro", "Rumänien", RoleTypes.Delegate);
                yield return new SimulationRole("ru", "Russische Föderation", RoleTypes.Delegate);
                yield return new SimulationRole("se", "Schweden", RoleTypes.Delegate);
                yield return new SimulationRole("rs", "Serbien", RoleTypes.Delegate);
                yield return new SimulationRole("sl", "Sierra Leone", RoleTypes.Delegate);
                yield return new SimulationRole("lk", "Sri Lanka", RoleTypes.Delegate);
                yield return new SimulationRole("uy", "Uruguay", RoleTypes.Delegate);
                yield return new SimulationRole("ae", "Vereinigte Arabische Emirate", RoleTypes.Delegate);
                yield return new SimulationRole("us", "Vereinigte Staaten", RoleTypes.Delegate);

                yield return new SimulationRole("na-12", "GNWP", RoleTypes.Ngo);
                yield return new SimulationRole("na-13", "Intern. Komitee v. Rotem Kreuz", RoleTypes.Ngo);

                yield return new SimulationRole("un", "Presse", RoleTypes.Spectator);
                yield return new SimulationRole("un", "Zuschauer", RoleTypes.Spectator);
                yield return new SimulationRole("un", "Ihre Exzellenz die Generalsekretärin", RoleTypes.Spectator);
            }
        }
    }
}