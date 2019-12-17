using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Hubs
{
    public class ResolutionHubHandler
    {
        IHubContext<ResolutionHub> _hub;

        public ResolutionHubHandler(IHubContext<ResolutionHub> hub)
        {
            _hub = hub;
        }

    }
}
