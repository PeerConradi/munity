using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Conference
{

    /// <summary>
    /// Use this if you want a group of users make custom applications but every one of this applications
    /// should be collected in one giant stack and looked at together in one big bundle.
    /// If you want multiple users to apply to the same Role or Delegations you can also use
    /// GroupApplication.
    /// <see cref="GroupApplication"/>
    ///
    /// <seealso cref="RoleApplication"/>
    /// </summary>
    public class GroupedRoleApplication
    {
        public int GroupedRoleApplicationId { get; set; }

        public string GroupName { get; set; }

        public List<RoleApplication> Applications { get; set; }

        public DateTime CreateTime { get; set; }

        [Timestamp]
        public byte[] GroupedRoleApplicationTimestamp { get; set; }
    }
}
