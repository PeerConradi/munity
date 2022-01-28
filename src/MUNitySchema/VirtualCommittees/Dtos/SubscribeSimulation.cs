using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.VirtualCommittees.Dtos
{
    /// <summary>
    /// Message Body to subscribe to a simulation and be added into the WebSocket-Group.
    /// the socket will be named sim_[SimulationId].
    /// </summary>
    public class SubscribeSimulation : SimulationRequest
    {
        /// <summary>
        /// The SignalR ConnectionId that should be added to the group.
        /// </summary>
        public string ConnectionId { get; set; }
    }
}
