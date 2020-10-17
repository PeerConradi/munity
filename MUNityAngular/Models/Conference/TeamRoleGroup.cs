using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MUNityAngular.Models.Conference.Roles;

namespace MUNityAngular.Models.Conference
{

    [DataContract]
    public class TeamRoleGroup
    {

        [DataMember]
        public int TeamRoleGroupId { get; set; }

        [MaxLength(150)]
        [DataMember]
        [Required]
        public string Name { get; set; }

        [MaxLength(250)]
        [DataMember]
        public string FullName { get; set; }

        [MaxLength(10)]
        [DataMember]
        public string Abbreviation { get; set; }

        [DataMember]
        public int GroupLevel { get; set; }

        [DataMember]
        public List<TeamRole> TeamRoles { get; set; }

        [Timestamp]
        [IgnoreDataMember]
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public byte[] TeamRoleGroupTimestamp { get; set; }
    }
}
