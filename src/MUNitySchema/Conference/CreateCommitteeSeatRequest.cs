using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MUNity.Schema.Conference
{
    public class CreateCommitteeSeatRequest
    {
        [Required]
        public string RoleName { get; set; }

        [Required]
        public string CommitteeId { get; set; }

        public int CountryId { get; set; } = -1;

        public string DelegationId { get; set; } = null;

        public string Subtype { get; set; }
    }

    public class CreateCommitteeSeatResponse : AbstractResponse
    {

        public int CreatedRoleId { get; set; }
    }
}
