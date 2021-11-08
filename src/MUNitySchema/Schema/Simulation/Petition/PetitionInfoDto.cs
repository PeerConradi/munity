using MUNity.Base;
using MUNity.Models.Simulation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Simulation
{
    public class PetitionInfoDto
    {
        public int AgendaItemId { get; set; }

        public string PetitionId { get; set; }

        public string SubmitterDisplayName { get; set; }

        public string SubmitterRoleName { get; set; }

        public string TypeName { get; set; }

        public int? OrderIndex { get; set; }

        public DateTime SubmitTime { get; set; }

        public EPetitionStates Status { get; set; }

        public string CategoryName { get; set; }

        public string RoleIso { get; set; } = "un";
    }
}
