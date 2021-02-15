using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.User
{
    /// <summary>
    /// Returns base information about a user. This schema can be different depending on the privacy settings of the user.
    /// </summary>
    public class UserInformation
    {
        /// <summary>
        /// Returns the username of the user. This will always be sent.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The first name of the user. This can also just be initals when the user set it in his/her privacy settings.
        /// </summary>
        public string Forename { get; set; }

        /// <summary>
        /// The last name of the user. This can also just be initials when the user set only display initals in his/her privacy Settings.
        /// </summary>
        public string Lastname { get; set; }

        /// <summary>
        /// The date when the user was last online. Can be null when the user selected to hide the last online date.
        /// </summary>
        public DateTime? LastOnline { get; set; }
    }
}
