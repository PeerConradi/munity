using MUNity.Schema.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.ViewModel
{
    public class SimulationUserContext
    {
        public int UserId { get; set; }

        public bool IsChair => RoleType == RoleTypes.Chairman;

        public string Token { get; set; }

        public string DisplayName { get; set; }

        public string RoleIso { get; set; }

        public string RoleName { get; set; }

        public RoleTypes RoleType { get; set; }
    }
}
