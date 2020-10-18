using MUNityAngular.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Models.Resolution.V2;

namespace MUNityAngular.Hubs
{

    /// <summary>
    /// This interface defines every function that can be send from the Server via a web-socket
    /// directly to the client.
    /// Every function that is definied in here must be created as a Task and have a context
    /// to the resolutions
    /// </summary>
    public interface ITypedResolutionHub
    {
        Task ResolutionChanged(ResolutionV2 resolution);
    }
}
