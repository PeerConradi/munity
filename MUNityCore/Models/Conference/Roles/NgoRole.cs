using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MUNityCore.Models.Conference.Roles
{

    /// <summary>
    /// A NGO-Role is the role of a non-governmental Organization on a conference.
    /// This Role is floating on the conference and unlike the real situation of the United Nations
    /// not bound to any committee (ECOSOC).
    /// Every NGO Role can be Part of one Group, this way you can create two Roles for the Same NGO
    /// for example Amnesty-International and group both roles by setting both Group Values to 1.
    /// The same way you can also group NGO-Roles with different names.
    /// </summary>
    public class NgoRole : AbstractRole
    {
        /// <summary>
        /// A Group for this NGO Role. Every other NGO Role of the conference
        /// with the same Group-Number is also part of this group.
        /// </summary>
        public int Group { get; set; }

        /// <summary>
        /// The name of the ngo. For Example Amnesty-International.
        /// </summary>
        [MaxLength(250)]
        public string NgoName { get; set; }

        /// <summary>
        /// Is this role the leaderRole of its group.
        /// </summary>
        public bool Leader { get; set; }

    }
}
