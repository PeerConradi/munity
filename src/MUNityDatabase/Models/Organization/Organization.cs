using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MUNity.Database.Interfaces;
using MUNity.Database.Models.Conference;
using MUNityCore.Models;

namespace MUNity.Database.Models.Organization;

[DataContract]
public class Organization : IIsDeleted
{
    public string OrganizationId { get; set; } = "";

    [MaxLength(150)]
    public string OrganizationName { get; set; }

    [MaxLength(18)]
    public string OrganizationShort { get; set; }

    public ICollection<OrganizationRole> Roles { get; set; }

    public ICollection<OrganizationMember> Member { get; set; }

    public ICollection<Project> Projects { get; set; }


    public Organization()
    {
        Roles = new List<OrganizationRole>();
        Member = new List<OrganizationMember>();
        Projects = new List<Project>();
    }

    public bool IsDeleted { get; set; }
}
