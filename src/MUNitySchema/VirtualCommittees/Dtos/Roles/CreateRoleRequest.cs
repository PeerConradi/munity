using MUNity.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.VirtualCommittees.Dtos
{
    public class CreateRoleRequest : SimulationRequest
    {
        public string Name { get; set; }

        public string Iso { get; set; }

        public RoleTypes RoleType { get; set; }
    }
}
