using MUNity.Database.Models.User;
using MUNityCore.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Organization;

/// <summary>
/// The membership of a registered user inside an organization that is hosting model united nations conferences.
/// </summary>
public class OrganizationMember
{

    public int OrganizationMemberId { get; set; }


    public MunityUser User { get; set; }


    public Organization Organization { get; set; }


    public OrganizationRole Role { get; set; }

    public DateTime JoinedDate { get; set; }

}
