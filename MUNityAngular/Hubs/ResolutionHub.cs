using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Hubs
{

    /// <summary>
    /// The ResolutionHub handles every client request that comes via the socket: "resasocket"
    /// Note that for most functions of creating, editing or deleting a resolution the 
    /// ResolutionController is used, to make sure the user is authenticated to use this socket.
    /// </summary>
    public class ResolutionHub : Hub<ITypedResolutionHub>
    {

    }
}
