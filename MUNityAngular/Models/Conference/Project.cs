using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Conference
{
    public class Project
    {

        [MaxLength(80)]
        public string ProjectId { get; set; }

        [MaxLength(200)]
        public string ProjectName { get; set; }

        [MaxLength(10)]
        public string ProjectAbbreviation { get; set; }

        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public Organisation.Organisation ProjectOrganisation { get; set; }

        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public List<Conference> Conferences { get; set; }

        [Timestamp]
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public byte[] ProjectTimestamp { get; set; }

        public Project()
        {
            ProjectId = Guid.NewGuid().ToString();
        }
    }
}
