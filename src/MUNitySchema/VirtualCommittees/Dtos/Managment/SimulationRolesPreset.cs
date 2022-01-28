using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.VirtualCommittees.Dtos
{
    public class SimulationRolesPreset
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<MUNity.VirtualCommittees.Dtos.SimulationRoleDto> Roles { get; set; }
    }
}
