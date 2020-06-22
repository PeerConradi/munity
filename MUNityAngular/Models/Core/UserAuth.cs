using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Core
{
    public class UserAuth
    {
        public int UserAuthId { get; set; }

        public User User { get; set; }

        public bool CanCreateOrganisation { get; set; }
    }
}
