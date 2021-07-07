using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MUNity.Database.Models.Conference;
using MUNityCore.Models;

namespace MUNity.Database.Models.Organization
{

    [DataContract]
    public class Organization
    {
        public string OrganizationId { get; set; }

        [MaxLength(150)]
        public string OrganizationName { get; set; }
        
        [MaxLength(18)]
        public string OrganizationShort { get; set; }

        public List<OrganizationRole> Roles { get; set; }

        public List<OrganizationMember> Member { get; set; }

        public List<Project> Projects { get; set; }

        [Timestamp]
        public byte[] OrganizationTimestamp { get; set; }

        public Organization()
        {
            this.OrganizationId = Guid.NewGuid().ToString();
            Roles = new List<OrganizationRole>();
            Member = new List<OrganizationMember>();
            Projects = new List<Project>();
        }
    }
}
