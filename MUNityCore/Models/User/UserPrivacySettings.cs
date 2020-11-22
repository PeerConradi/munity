using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.IO;

namespace MUNityCore.Models.User
{
    public class UserPrivacySettings
    {

        /// <summary>
        /// The privacy settings of an user. This is privacy by default so everything should be kept as low
        /// as possible when creating the account.
        /// </summary>
        public enum ENameDisplayMode
        {
            /// <summary>
            /// Will show the Full Name: Max Mustermann
            /// </summary>
            FullName,

            /// <summary>
            /// Will display the complete forename and the first letter of the last name:
            /// Max M.
            /// </summary>
            FullForenameAndFirstLetterLastName,

            /// <summary>
            /// Will display the first letter of the forename and the complete lastname:
            /// M. Mustermann
            /// </summary>
            FirstLetterForenameFullLastName,

            /// <summary>
            /// Will display only the initals of the user:
            /// M. M.
            /// </summary>
            Initals
        }

        public enum EDisplayAuthMode
        {
            /// <summary>
            /// Everyone can see this information
            /// </summary>
            Everyone,

            /// <summary>
            /// Logged in users can see this information
            /// </summary>
            Users,

            /// <summary>
            /// only users that are added as friends can see this information
            /// </summary>
            Friends,

            /// <summary>
            /// only I can see this information.
            /// </summary>
            JustMe
        }
        
        public int UserPrivacySettingsId { get; set; }

        public int UserRef { get; set; }

        public MunityUser User { get; set; }

        /// <summary>
        /// The way the User Forename and Lastname will be given out, to an not logged in user.
        /// </summary>
        public ENameDisplayMode PublicNameDisplayMode { get; set; } = ENameDisplayMode.Initals;

        /// <summary>
        /// The way the User Forename and lastname will be shown to a user that is logged in
        /// into the backend. Note that this will be used on all types of users. Moderators
        /// and Admins can view the full name anyway! Also the Team of a conference will
        /// also be able to see the complete user data.
        /// </summary>
        public ENameDisplayMode InternalNameDisplayMode { get; set; } =
            ENameDisplayMode.FullForenameAndFirstLetterLastName;

        /// <summary>
        /// The way other users of a conference you are in will be able to see this
        /// users fore- and lastname
        /// </summary>
        public ENameDisplayMode ConferenceNameDisplayMode { get; set; } = ENameDisplayMode.FullName;

        /// <summary>
        /// The history of conferences this user participated in
        /// </summary>
        public EDisplayAuthMode ConferenceHistory { get; set; } = EDisplayAuthMode.JustMe;

        /// <summary>
        /// The friends this user has added
        /// </summary>
        public EDisplayAuthMode Friendslist { get; set; } = EDisplayAuthMode.JustMe;

        /// <summary>
        /// Who is allowed to see the pinboard of the user.
        /// </summary>
        public EDisplayAuthMode Pinboard { get; set; } = EDisplayAuthMode.Friends;

        /// <summary>
        /// The age of this user.
        /// </summary>
        public EDisplayAuthMode Age { get; set; } = EDisplayAuthMode.JustMe;
    }
}
