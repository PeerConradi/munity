using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Schema.Request.Organisation
{
    public class CreateOrganisationRequest
    {
        [Required]
        [MaxLength(300)]
        public string OrganisationName { get; set; }

        [Required]
        [MaxLength(16)]
        public string Abbreviation { get; set; }
    }
}
