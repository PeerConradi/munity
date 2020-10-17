using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MUNityAngular.Models.Conference;
using MUNityAngular.Models.Core;

namespace MUNityAngular.Models.Organisation
{

    [DataContract]
    public class Organisation
    {

        [DataMember]
        public string OrganisationId { get; set; }

        [DataMember]
        [MaxLength(150)]
        public string OrganisationName { get; set; }
        
        [DataMember]
        [MaxLength(18)]
        public string OrganisationAbbreviation { get; set; }

        [IgnoreDataMember]
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public List<OrganisationRole> Roles { get; set; }

        [IgnoreDataMember]
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public List<OrganisationMember> Member { get; set; }

        [IgnoreDataMember]
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public List<Project> Projects { get; set; }

        public Organisation()
        {
            this.OrganisationId = Guid.NewGuid().ToString();
            Roles = new List<OrganisationRole>();
            Member = new List<OrganisationMember>();
            Projects = new List<Project>();
        }
    }
}
