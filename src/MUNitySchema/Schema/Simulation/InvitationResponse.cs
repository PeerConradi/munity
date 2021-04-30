using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Simulation
{
    public class InvitationResponse
    {
        public int SimulationUserId { get; set; }

        public string Token { get; set; }

        public string DisplayName { get; set; }

        public int SimulationId { get; set; }

        public string SimulationName { get; set; }

        public string RoleName { get; set; }

        public string RoleIso { get; set; }
    }
}
