using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Conference.Roles
{

    /// <summary>
    /// The abstract role provides base functionality for the different types of roles
    /// Every role as a name and a full name. Examples for role names could be. Head of organization.
    /// project leader, team leader, participant support etc.
    ///
    /// Every role is part of one conference. You cannot use the exact same role inside another conference but you can
    /// use templates to create a general structure of a conference.
    /// <seealso cref="Roles.ConferenceDelegateRole"/>
    /// <seealso cref="Roles.ConferenceNgoRole"/>
    /// <seealso cref="Roles.ConferencePressRole"/>
    /// <seealso cref="Roles.ConferenceSecretaryGeneralRole"/>
    /// <seealso cref="Roles.ConferenceTeamRole"/>
    /// <seealso cref="Roles.ConferenceVisitorRole"/>
    /// </summary>
    public abstract class AbstractConferenceRole
    {

        /// <summary>
        /// The Id of the Role.
        /// </summary>
        [Key]
        public int RoleId { get; set; }

        /// <summary>
        /// The short name of the role
        /// </summary>
        [MaxLength(150)]
        public string RoleName { get; set; }

        /// <summary>
        /// the long Name of the role
        /// </summary>
        [MaxLength(250)]
        public string RoleFullName { get; set; }

        /// <summary>
        /// A short for the role. for example PL for project leader.
        /// </summary>
        [MaxLength(10)]
        public string RoleShort { get; set; }

        /// <summary>
        /// The conference that this role is assigned to.
        /// </summary>
        public Conference Conference { get; set; }

        /// <summary>
        /// the authorization of this role.
        /// </summary>
        public RoleAuth RoleAuth { get; set; }

        /// <summary>
        /// an icon name for the role. the icon that will be displayed has to be chosen by
        /// the frontend.
        /// </summary>
        [MaxLength(250)]
        public string IconName { get; set; }

        /// <summary>
        /// The application state. It gives information if a user can apply for this role or not.
        /// It also provides information about the type of application progress, for example can
        /// only someone that is part of the organization that holds this conference apply for this role
        /// or anyone. Its also possible to only let users with an account on the platform apply for a role.
        /// </summary>
        public EApplicationStates ApplicationState { get; set; } = EApplicationStates.Closed;

        /// <summary>
        /// an extended value for the application state.
        /// </summary>
        [MaxLength(250)]
        public string ApplicationValue { get; set; }

        /// <summary>
        /// Does this role allow more then one application and the project leader has to choose
        /// which application and user he picks for this role. If this is turned to false it is
        /// first come - first served principal and the application mode will close when someone has applied.
        /// </summary>
        public bool AllowMultipleParticipations { get; set; } = false;

        /// <summary>
        /// The type of the role. This will most likely return "DelegateRole", "NgoRole", "PressRole",
        /// "SecreataryGeneralRole", "TeamRole" or "VisitorRole".
        /// This field is used for the concurrencyToken
        /// </summary>
        [MaxLength(150)]
       
        public string RoleType { get; [Obsolete("This is the concurrencyToken please only read")]set; }

        /// <summary>
        /// The timestamp when this role had been created or last changed.
        /// </summary>
        [Timestamp]
        public byte[] RoleTimestamp { get; set; }
    }
}
