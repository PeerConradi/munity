using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Models.SimSim;

namespace MUNityAngular.Hubs
{
    public interface ITypedSimulationHub
    {
        Task UserJoined(ISimSimUserFacade user);

        Task UserLeft(ISimSimUserFacade user);

        Task ChatMessageAdded(AllChatMessage message);
    }
}
