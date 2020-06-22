using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Models.Conference;
using MUNityAngular.Models.Core;

namespace MUNityAngular.Models.Organisation
{
    public class Organisation
    {
        public string OrganisationId { get; set; }

        public string OrganisationName { get; set; }

        public string OrganisationAbbreviation { get; set; }

        public List<OrganisationRole> Roles { get; set; }

        public List<OrganisationMember> Member { get; set; }

        public List<Project> Projects { get; set; }

        public Organisation()
        {
            this.OrganisationId = Guid.NewGuid().ToString();
        }
    }
}
