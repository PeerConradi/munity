using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Conference
{
    public class ApplicationAvailableDelegation
    {
        public string DelegationId { get; set; }

        public string Name { get; set; }

        public List<ApplicationDelegationRoleSlot> Roles { get; set; }
    }

    public class ApplicationDelegationRoleSlot
    {
        public string RoleName { get; set; }

        public string CommitteeName { get; set; }

        public decimal Costs { get; set; }

        public string CountryName { get; set; }

        public string CountryIso { get; set; }
    }
}
