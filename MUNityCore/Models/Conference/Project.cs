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
    /// An organization can host different projects. For example could the project group
    /// all conferences inside a specific city over the years:
    /// Model United Nations Berlin 2015
    /// Model United Nations Berlin 2016
    ///
    /// The organization could also group different styles of conferences inside a project
    /// for example:
    /// Model United Nations in the classroom
    /// Model United Nations in the university
    /// </summary>
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
        public Organization.Organization ProjectOrganization { get; set; }

        [DataMember]
        [NotMapped]
        public string ProjectOrganizationId => ProjectOrganization?.OrganizationId ?? null;

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
