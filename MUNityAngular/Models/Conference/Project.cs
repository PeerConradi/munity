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
    [DataContract]
    public class Project
    {
        [DataMember]
        public string ProjectId { get; set; }

        [DataMember]
        [MaxLength(200)]
        public string ProjectName { get; set; }

        [DataMember]
        [MaxLength(10)]
        public string ProjectAbbreviation { get; set; }

        [IgnoreDataMember]
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public Organisation.Organisation ProjectOrganisation { get; set; }

        [DataMember]
        [NotMapped]
        public string ProjectOrganisationId => ProjectOrganisation?.OrganisationId ?? null;

        public List<Conference> Conferences { get; set; }

        [IgnoreDataMember]
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [Timestamp]
        public byte[] ProjectTimestamp { get; set; }

        public Project()
        {
            ProjectId = Guid.NewGuid().ToString();
        }
    }
}
