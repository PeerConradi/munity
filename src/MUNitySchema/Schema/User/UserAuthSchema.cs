using MUNity.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.User
{
    /// <summary>
    /// The auth Schema of a MUNity User.
    /// </summary>
    public class UserAuthSchema
    {
        

        /// <summary>
        /// The AuthId
        /// </summary>
        public int UserAuthId { get; set; }

        /// <summary>
        /// The name of the auth.
        /// </summary>
        public string UserAuthName { get; set; }

        /// <summary>
        /// Can this user Auth create an Organization.
        /// </summary>
        public bool CanCreateOrganization { get; set; }

        /// <summary>
        /// The auth level.
        /// </summary>
        public EAuthLevel AuthLevel { get; set; }
    }
}
