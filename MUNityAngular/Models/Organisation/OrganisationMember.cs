using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Organisation
{
    public class OrganisationMember
    {
        public int OrganisationMemberId { get; set; }

        public Core.User User { get; set; }

        public Organisation Organisation { get; set; }

        public OrganisationRole Role { get; set; }
    }
}
