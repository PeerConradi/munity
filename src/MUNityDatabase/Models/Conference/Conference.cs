using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MUNity.Database.Models.Conference;
using MUNity.Database.Models.Conference.Roles;
using MUNity.Database.Models.User;
using MUNityBase;
using MUNityCore.Models.User;

namespace MUNity.Database.Models.Conference;

/// <summary>
/// A conference is as the name tells a single conference.
/// Like Model United Nations London 2021. This means it is a single
/// event of maybe a list of Model United Nation Conferences. If there are more
/// the conference, for example a conference every year, then this will can be
/// found by the Project
/// </summary>
public class Conference
{

    [MaxLength(80)] public string ConferenceId { get; set; } = "";

    [MaxLength(150)]
    public string Name { get; set; }

    [MaxLength(250)]
    public string FullName { get; set; }

    [MaxLength(18)]
    public string ConferenceShort { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime CreationDate { get; set; }

    public MunityUser CreationUser { get; set; }

    public ConferenceApplicationOptions ApplicationOptions { get; set; }

    public ICollection<Committee> Committees { get; set; }

    public ICollection<AbstractConferenceRole> Roles { get; set; }

    public ICollection<TeamRoleGroup> TeamRoleGroups { get; set; }

    public ICollection<Delegation> Delegations { get; set; }

    public Project ConferenceProject { get; set; }

    public EConferenceVisibilityMode Visibility { get; set; }

    public decimal GeneralParticipationCost { get; set; }

    public Conference()
    {
        Committees = new List<Committee>();
        TeamRoleGroups = new List<TeamRoleGroup>();
        Roles = new List<AbstractConferenceRole>();
        Visibility = EConferenceVisibilityMode.Users;
        CreationDate = DateTime.Now;
    }
}
