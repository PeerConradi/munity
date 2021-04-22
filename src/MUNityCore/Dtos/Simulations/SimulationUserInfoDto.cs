using MUNity.Schema.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Dtos.Simulations
{
    public class SimulationUserInfoDto
    {
        public int SimulationUserId { get; set; }

        public string DisplayName { get; set; }

        public string RoleName { get; set; }

        public string RoleIso { get; set; }

        public RoleTypes RoleType { get; set; }

        public bool IsOnline { get; set; }
    }
}
