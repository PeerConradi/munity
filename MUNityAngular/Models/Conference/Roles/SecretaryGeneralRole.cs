using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Conference.Roles
{
    public class SecretaryGeneralRole : AbstractRole
    {
        public string Title { get; set; }

        public override string RoleType => this.GetType().Name;
    }
}
