using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Conference
{

    /// <summary>
    /// If a user applies to a Role this will create a RoleApplication.
    /// When an Application is Created to the Role there is a Link between the Role itself
    /// The User that made the application plus some more information of this application
    /// </summary>
    public class RoleApplication
    {
        public int RoleApplicationId { get; set; }

        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public Core.User User { get; set; }

        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public AbstractRole Role { get; set; }

        public DateTime ApplyDate { get; set; }

        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(4096)]
        public string Content { get; set; }

        [Timestamp]
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public byte[] RoleApplicationTimestamp { get; set; }
    }
}
