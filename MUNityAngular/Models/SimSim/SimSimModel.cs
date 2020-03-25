using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.DataHandlers.EntityFramework.Models;

namespace MUNityAngular.Models.SimSim
{
    public class SimSimModel : ISimSimFacade
    {
        public int SimSimId { get; set; }

        public string Name { get; set; }

        public List<Delegation> Delegations { get; set; }

        public List<AllChatMessage> AllChat { get; set; }
    }
}
