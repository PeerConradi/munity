using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Organisation
{
    public class OrganisationRole
    {
        public int OrganisationRoleId { get; set; }

        public string RoleName { get; set; }

        public Organisation Organisation { get; set; }

        public bool CanCreateProject { get; set; }
    }
}
