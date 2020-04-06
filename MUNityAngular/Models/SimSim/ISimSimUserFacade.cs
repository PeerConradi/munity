using MUNityAngular.DataHandlers.EntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.SimSim
{
    public interface ISimSimUserFacade
    {
        string UserToken { get; set; }

        string DisplayName { get; set; }

        Delegation Delegation { get; set; }

        bool IsChair { get; set; }
    }
}
