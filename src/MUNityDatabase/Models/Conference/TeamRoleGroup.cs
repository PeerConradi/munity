﻿using MUNity.Database.Models.Conference.Roles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Conference;

/// <summary>
/// A team role group is as the name suggests a grouping of team roles.
/// For example could different organizational team roles be grouped into
/// "main organization team".
/// </summary>
public class TeamRoleGroup
{

    public int TeamRoleGroupId { get; set; }

    [MaxLength(150)]
    [Required]
    public string Name { get; set; }

    [MaxLength(250)]
    public string FullName { get; set; }

    [MaxLength(10)]
    public string TeamRoleGroupShort { get; set; }

    public int GroupLevel { get; set; }

    public Conference Conference { get; set; }

    public ICollection<ConferenceTeamRole> TeamRoles { get; set; } = new List<ConferenceTeamRole>();

}
