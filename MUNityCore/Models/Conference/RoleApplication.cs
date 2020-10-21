using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUNityCore.Models.Conference
{

    /// <summary>
    /// If a user applies to a Role this will create a RoleApplication.
    /// When an Application is Created to the Role there is a Link between the Role itself
    /// The User that made the application plus some more information of this application
    /// </summary>
    [DataContract]
    public class RoleApplication
    {
        [DataMember]
        public int RoleApplicationId { get; set; }

        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [IgnoreDataMember]
        public Core.User User { get; set; }

        [NotMapped]
        public string Username => User?.Username ?? "";

        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [IgnoreDataMember]
        public AbstractRole Role { get; set; }

        [NotMapped]
        public int RoleId => Role?.RoleId ?? -1;

        [DataMember]
        public DateTime ApplyDate { get; set; }

        [DataMember]
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        
        [DataMember]
        public string Content { get; set; }

        [Timestamp]
        [IgnoreDataMember]
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public byte[] RoleApplicationTimestamp { get; set; }
    }
}
