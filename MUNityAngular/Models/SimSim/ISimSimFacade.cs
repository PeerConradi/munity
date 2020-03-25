using MUNityAngular.DataHandlers.EntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.SimSim
{
    interface ISimSimFacade
    {

        int SimSimId { get; set; }
        string Name { get; set; }

        List<Delegation> Delegations { get; set; }
    }
}
