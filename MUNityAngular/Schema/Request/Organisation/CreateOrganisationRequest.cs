using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Schema.Request.Organisation
{
    public class CreateOrganisationRequest
    {
        [Required]
        
        public string OrganisationName { get; set; }

        [Required]
        public string Abbreviation { get; set; }
    }
}
