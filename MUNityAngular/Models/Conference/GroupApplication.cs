using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Conference
{

    /// <summary>
    /// When users want to apply as a group it is possible to create a collective Application
    /// for the same AbstractRole. If you want to create custom Applications for multiple Roles
    /// but every and ever user of the group should have a custom Application use GroupedRoleApplication.
    /// </summary>
    [DataContract]
    public class GroupApplication
    {

        [DataMember]
        public int GroupApplicationId { get; set; }


        [IgnoreDataMember]
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public List<Core.User> Users { get; set; }

        [IgnoreDataMember]
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public AbstractRole Role { get; set; }

        [IgnoreDataMember]
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public Delegation Delegation { get; set; }

        [MaxLength(150)]
        [Required]
        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Content { get; set; }

        [DataMember]
        public DateTime ApplicationDate { get; set; }

        [Timestamp]
        [IgnoreDataMember]
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public byte[] GroupApplicationTimestamp { get; set; }

        public GroupApplication()
        {
            Users = new List<Core.User>();
        }
    }
}
