using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.VirtualCommittee.Dtos
{
    public class SimulationUserTokenRequest : SimulationRequest
    {
        /// <summary>
        /// The id of the user you want to know the token of.
        /// </summary>
        public int UserId { get; set; }
    }
}
