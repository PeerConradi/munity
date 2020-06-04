using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Conference.Roles
{
    public class PressRole : AbstractRole
    {
        public enum EPressCategories
        {
            Print,
            TV,
            Online
        }

        public EPressCategories PressCategory { get; set; }
    }
}
