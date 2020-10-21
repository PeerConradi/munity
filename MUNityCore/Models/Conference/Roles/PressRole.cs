using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MUNityCore.Models.Conference.Roles
{

    [DataContract]
    public class PressRole : AbstractRole
    {
        public enum EPressCategories
        {
            Print,
            TV,
            Online
        }

        [DataMember]
        public EPressCategories PressCategory { get; set; }

    }
}
