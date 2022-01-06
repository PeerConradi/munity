using MUNity.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.VirtualCommittee.Dtos
{
    public class SimulationSlotDto
    {
        public int SimulationUserId { get; set; }

        public string DisplayName { get; set; }

        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public string RoleIso { get; set; }

        public RoleTypes RoleType { get; set; }

        public bool IsOnline { get; set; }

        public bool CanCreateRole { get; set; }

        public bool CanSelectRole { get; set; }

        public bool CanEditResolution { get; set; }

        public bool CanEditListOfSpeakers { get; set; }
    }
}
