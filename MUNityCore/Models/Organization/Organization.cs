using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MUNityCore.Models.Core;
using MUNityCore.Models.Conference;

namespace MUNityCore.Models.Organization
{

    [DataContract]
    public class Organization
    {

        [DataMember]
        public string OrganizationId { get; set; }

        [DataMember]
        [MaxLength(150)]
        public string OrganizationName { get; set; }
        
        [DataMember]
        [MaxLength(18)]
        public string OrganizationAbbreviation { get; set; }

        [IgnoreDataMember]
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public List<OrganizationRole> Roles { get; set; }

        [IgnoreDataMember]
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public List<OrganizationMember> Member { get; set; }

        [IgnoreDataMember]
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public List<Project> Projects { get; set; }

        public Organization()
        {
            this.OrganizationId = Guid.NewGuid().ToString();
            Roles = new List<OrganizationRole>();
            Member = new List<OrganizationMember>();
            Projects = new List<Project>();
        }
    }
}
