using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using MUNity.Database.Context;
using MUNity.Database.General;
using MUNity.Database.Models.Conference;
using MUNity.Database.Models.Conference.Roles;
using MUNity.Database.Models.Organization;
using MUNity.Database.Models.User;

namespace MUNity.Database.FluentAPI;

public static class OrganizationFluentExtensions
{

    public static int AddBasicConferenceAuthorizations(this MunityContext context, string conferenceId)
    {
        var conference = context.Conferences.FirstOrDefault(n => n.ConferenceId == conferenceId);
        var ownerAuth = new ConferenceRoleAuth()
        {
            Conference = conference,
            CanEditConferenceSettings = true,
            CanEditParticipations = true,
            CanSeeApplications = true,
            PowerLevel = 1,
            RoleAuthName = "Project-Owner"
        };
        context.ConferenceRoleAuthorizations.Add(ownerAuth);

        var participantControllingTeamAuth = new ConferenceRoleAuth()
        {
            Conference = conference,
            CanEditConferenceSettings = false,
            CanEditParticipations = true,
            CanSeeApplications = true,
            PowerLevel = 2,
            RoleAuthName = "Team (Participant Management)"
        };
        context.ConferenceRoleAuthorizations.Add(participantControllingTeamAuth);

        var teamAuth = new ConferenceRoleAuth()
        {
            Conference = conference,
            CanEditConferenceSettings = false,
            CanEditParticipations = false,
            CanSeeApplications = true,
            RoleAuthName = "Team (Basic)",
            PowerLevel = 3,
        };
        context.ConferenceRoleAuthorizations.Add(teamAuth);

        var participantAuth = new ConferenceRoleAuth()
        {
            Conference = conference,
            CanEditConferenceSettings = false,
            RoleAuthName = "Participant",
            PowerLevel = 4,
            CanSeeApplications = false,
            CanEditParticipations = false
        };
        context.ConferenceRoleAuthorizations.Add(participantAuth);

        return context.SaveChanges();
    }






}
