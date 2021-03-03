using MUNity.Schema.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.Simulation.Presets
{
    public class GV_Preset : ISimulationPreset
    {
        public string Id => "gv";

        public string Name => "Generalversammlung";

        public IEnumerable<SimulationRole> Roles
        {
            get
            {
                yield return new SimulationRole("un", "Vorsitzende(r)", RoleTypes.Chairman);
                yield return new SimulationRole("dz", "Algerien", RoleTypes.Delegate);
                yield return new SimulationRole("ar", "Argentinien", RoleTypes.Delegate);
                yield return new SimulationRole("am", "Armenien", RoleTypes.Delegate);
                yield return new SimulationRole("au", "Australien", RoleTypes.Delegate);
                yield return new SimulationRole("be", "Belgien", RoleTypes.Delegate);
                yield return new SimulationRole("bw", "Botswana", RoleTypes.Delegate);
                yield return new SimulationRole("bi", "Burundi", RoleTypes.Delegate);
                yield return new SimulationRole("cn", "China", RoleTypes.Delegate);
                yield return new SimulationRole("dk", "Dänemark", RoleTypes.Delegate);
                yield return new SimulationRole("do", "Dominikanische Republik", RoleTypes.Delegate);
                yield return new SimulationRole("ec", "Ecuador", RoleTypes.Delegate);
                yield return new SimulationRole("fj", "Fidschi", RoleTypes.Delegate);
                yield return new SimulationRole("fr", "Frankreich", RoleTypes.Delegate);
                yield return new SimulationRole("ga", "Gabun", RoleTypes.Delegate);
                yield return new SimulationRole("ge", "Georgien", RoleTypes.Delegate);
                yield return new SimulationRole("gn", "Guinea", RoleTypes.Delegate);
                yield return new SimulationRole("in", "Indien", RoleTypes.Delegate);
                yield return new SimulationRole("jp", "Japan", RoleTypes.Delegate);
                yield return new SimulationRole("ye", "Jemen", RoleTypes.Delegate);
                yield return new SimulationRole("jo", "Jordanien", RoleTypes.Delegate);
                yield return new SimulationRole("kh", "Kambodscha", RoleTypes.Delegate);
                yield return new SimulationRole("ki", "Kiribati", RoleTypes.Delegate);
                yield return new SimulationRole("cg", "Republik Kongo", RoleTypes.Delegate);
                yield return new SimulationRole("cu", "Kuba", RoleTypes.Delegate);
                yield return new SimulationRole("lr", "Liberia", RoleTypes.Delegate);
                yield return new SimulationRole("ly", "Libyen", RoleTypes.Delegate);
                yield return new SimulationRole("mg", "Madagaskar", RoleTypes.Delegate);
                yield return new SimulationRole("ma", "Marokko", RoleTypes.Delegate);
                yield return new SimulationRole("mm", "Myanmar", RoleTypes.Delegate);
                yield return new SimulationRole("ni", "Nicaragua", RoleTypes.Delegate);
                yield return new SimulationRole("ne", "Niger", RoleTypes.Delegate);
                yield return new SimulationRole("om", "Oman", RoleTypes.Delegate);
                yield return new SimulationRole("pa", "Panama", RoleTypes.Delegate);
                yield return new SimulationRole("pg", "Papua-Neuguinea", RoleTypes.Delegate);
                yield return new SimulationRole("py", "Paraguay", RoleTypes.Delegate);
                yield return new SimulationRole("pe", "Peru", RoleTypes.Delegate);
                yield return new SimulationRole("ru", "Russische Föderation", RoleTypes.Delegate);
                yield return new SimulationRole("se", "Schweden", RoleTypes.Delegate);
                yield return new SimulationRole("sk", "Slowakei", RoleTypes.Delegate);
                yield return new SimulationRole("so", "Somalia", RoleTypes.Delegate);
                yield return new SimulationRole("lk", "Sri Lanka", RoleTypes.Delegate);
                yield return new SimulationRole("tg", "Togo", RoleTypes.Delegate);
                yield return new SimulationRole("cz", "Tschechische Republik", RoleTypes.Delegate);
                yield return new SimulationRole("ua", "Ukraine", RoleTypes.Delegate);
                yield return new SimulationRole("hu", "Ungarn", RoleTypes.Delegate);
                yield return new SimulationRole("ve", "Venezuela", RoleTypes.Delegate);
                yield return new SimulationRole("ae", "Vereinigte Arabische Emirate", RoleTypes.Delegate);
                yield return new SimulationRole("gb", "Großbritannien", RoleTypes.Delegate);
                yield return new SimulationRole("us", "Vereinigte Staaten", RoleTypes.Delegate);

                yield return new SimulationRole("na-01", "Freedom House", RoleTypes.Ngo);
                yield return new SimulationRole("na-02", "Greenpeace International", RoleTypes.Ngo);
                yield return new SimulationRole("na-03", "UN Women", RoleTypes.Ngo);

                yield return new SimulationRole("un", "Presse", RoleTypes.Spectator);
                yield return new SimulationRole("un", "Zuschauer", RoleTypes.Spectator);
                yield return new SimulationRole("un", "Ihre Exzellenz die Generalsekretärin", RoleTypes.Spectator);
            }
        }
    }
}