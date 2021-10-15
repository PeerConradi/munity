using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using MUNity.Database.General;

namespace MUNity.Database.Models.Conference.Roles
{

    /// <summary>
    /// A Delegate Role is an attendee role that everyone can have that is part of a delegation
    /// and represents some sort of state/country at the conference. The delegation can be
    /// the same country/state but is not necessarily. You can also create multiply DelegationWishes Roles
    /// of different states and put them together into a DelegationWishes named Presidents of Europe.
    ///
    /// Every Delegate Role takes a seat inside a Committee
    /// </summary>
    public class ConferenceDelegateRole : AbstractConferenceRole
    {

        /// <summary>
        /// The Country that this Delegate Role represents.
        /// </summary>
        public Country DelegateCountry { get; set; }

        /// <summary>
        /// Is this Role a Leader Role of the DelegationWishes
        /// </summary>
        public bool IsDelegationLeader { get; set; }

        /// <summary>
        /// The Title of this DelegationWishes Role. This is not the Role Name itself
        /// The RoleName can be something like: Delegate and the Title is Delegate of Germany in the Assembly General
        /// </summary>

        public string Title { get; set; }

        /// <summary>
        /// A DelegationWishes that this Role is Part for example the DelegationWishes of Germany.
        /// </summary>
        public Delegation Delegation { get; set; }

        /// <summary>
        /// The committee that this Role is seated in.
        /// </summary>
        public Committee Committee { get; set; }

        /// <summary>
        /// An additional description of the delegate type, for example:
        /// delegate, president, press, non-goverment etc.
        /// </summary>
        public string DelegateType { get; set; }

    }
}
