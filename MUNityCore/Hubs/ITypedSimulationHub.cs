﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityCore.DataHandlers.EntityFramework.Models;
using MUNityCore.Models.Conference;
using MUNityCore.Models.Simulation;

namespace MUNityCore.Hubs
{
    public interface ITypedSimulationHub
    {
        Task UserJoined(SimulationUser user);

        Task UserLeft(SimulationUser user);

        Task ChatMessageAdded(AllChatMessage message);

        Task UserChangedDelegation(string usertoken, Delegation delegation);

        Task UserChangedRole(string usertoken, string role);

        Task RequestAdded(SimSimRequestModel request);

        Task RequestRemoved(SimSimRequestModel request);

        Task RequestsChanged(List<SimSimRequestModel> requests);
    }
}
