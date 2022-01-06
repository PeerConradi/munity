using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.VirtualCommittee.Dtos
{
    public class SimulationRolesPreset
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<MUNity.VirtualCommittee.Dtos.SimulationRoleDto> Roles { get; set; }
    }
}
