using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Models.SimSim;
using MUNityAngular.DataHandlers.EntityFramework.Models;

namespace MUNityAngular.Hubs
{
    public interface ITypedSimulationHub
    {
        Task UserJoined(ISimSimUserFacade user);

        Task UserLeft(ISimSimUserFacade user);

        Task ChatMessageAdded(AllChatMessage message);

        Task UserChangedDelegation(string usertoken, Delegation delegation);
    }
}
