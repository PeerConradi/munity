using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Conference.Roles
{
    public class NgoRole : AbstractRole
    {
        public int Group { get; set; }

        public bool Leader { get; set; }
    }
}
