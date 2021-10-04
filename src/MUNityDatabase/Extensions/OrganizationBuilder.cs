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

namespace MUNity.Database.Extensions
{
    public static class OrganizationFluentExtensions
    {
        public static Organization AddOrganization(this MunityContext context, Action<OrganizationOptionsBuilder> options)
        {
            var builder = new OrganizationOptionsBuilder(context);
            options(builder);
            var orga = builder.Organization;
            context.Organizations.Add(orga);
            context.SaveChanges();
            return orga;
        }

        public static OrganizationMember AddMemberIntoRole(this OrganizationRole role, MunityUser user)
        {
            var membership = new OrganizationMember();
            membership.Role = role;
            membership.User = user;
            membership.JoinedDate = DateTime.Now;
            return membership;
        }

        public static OrganizationMember AddMemberIntoRole(this MunityContext context, OrganizationRole role,
            MunityUser user)
        {
            var membership = context.OrganizationMembers.FirstOrDefault(n => n.User.Id == user.Id &&
                                                                          n.Role.OrganizationRoleId ==
                                                                          role.OrganizationRoleId);
            if (membership != null)
                return membership;

            membership = role.AddMemberIntoRole(user);
            context.OrganizationMembers.Add(membership);
            context.SaveChanges();
            return membership;
        }

        public static Project AddProject(this MunityContext context, Action<ProjectOptionsBuilder> options)
        {
            var builder = new ProjectOptionsBuilder(context);
            options(builder);
            var project = builder.Project;
            context.Projects.Add(project);
            context.SaveChanges();
            return project;
        }

        public static Conference AddConference(this MunityContext context, Action<ConferenceOptionsBuilder> options)
        {
            var builder = new ConferenceOptionsBuilder(context);
            options(builder);
            context.Conferences.Add(builder.Conference);
            context.SaveChanges();
            return builder.Conference;
        }

        public static TeamRoleGroup AddTeamRoleGroup(this Conference conference, Action<ITeamRoleBuilder> options)
        {
            var builder = new TeamRoleGroupBuilder();
            options(builder);
            conference.TeamRoleGroups.Add(builder.Group);
            if (builder.Group.TeamRoles.Count > 0)
            {
                foreach (var conferenceTeamRole in builder.Group.TeamRoles)
                {
                    conferenceTeamRole.Conference = conference;
                }
            }
            return builder.Group;
        }

        public static Committee AddCommittee(this Conference conference, Action<CommitteeOptionsBuilder> options)
        {
            var builder = new CommitteeOptionsBuilder();
            options(builder);
            builder.Committee.Conference = conference;

            var committeeEasy = Util.IdGenerator.AsPrimaryKey(builder.Committee.CommitteeShort);
            builder.Committee.CommitteeId = conference.ConferenceId + "-" + committeeEasy;

            if (builder.Committee.ChildCommittees.Count > 0)
            {
                foreach (var committeeChildCommittee in builder.Committee.ChildCommittees)
                {
                    committeeChildCommittee.Conference = conference;
                }
            }
            conference.Committees.Add(builder.Committee);
            return builder.Committee;
        }

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

        public static ConferenceDelegateRole AddSeat(this MunityContext context, string committeeId, string name, int? countryId = null, string shortName = null, string subTypeName = "Participant")
        {
            var committee = context.Committees.Include(n => n.Conference)
                .FirstOrDefault(n => n.CommitteeId == committeeId);

            if (committee == null)
                throw new ArgumentException($"The committe with the given id {committeeId} was not found!");

            var participantAuth = context.ConferenceRoleAuthorizations.FirstOrDefault(n =>
                n.RoleAuthName == subTypeName && n.Conference.ConferenceId == committee.Conference.ConferenceId);

            Country country = null;
            if (countryId.HasValue)
            {
                country = context.Countries.FirstOrDefault(n => n.CountryId == countryId);
                if (country == null)
                    throw new ArgumentException($"The given country with id: {countryId} was not found!");
            }

            if (participantAuth == null)
                throw new ArgumentException($"The given authorization was not found!");

            var role = new ConferenceDelegateRole()
            {
                Committee = committee,
                Conference = committee.Conference,
                ConferenceRoleAuth = participantAuth,
                RoleName = name,
                DelegateCountry = country,
                RoleFullName = name,
                DelegateType = subTypeName,
                RoleShort = shortName
            };

            context.Delegates.Add(role);
            context.SaveChanges();
            return role;
        }

        public static void GroupRolesOfCountryIntoADelegation(this MunityContext context, string conferenceId, int countryId,
            string delegationName, string delegationFullName = null, string delegationShortName = null)
        {
            var conference = context.Conferences.FirstOrDefault(n => n.ConferenceId == conferenceId);
            if (conference == null)
                throw new NullReferenceException($"The conference with id '{conferenceId}' was not found!");

            if (delegationFullName == null)
                delegationFullName = "Delegation " + delegationName;

            var delgation = new Delegation()
            {
                Conference = conference,
                Name = delegationName,
                Roles = context.Delegates
                    .Where(n => n.DelegateCountry.CountryId == countryId && n.Conference.ConferenceId == conferenceId).ToList(),
                FullName = "Delegation Deutschland",
                DelegationShort = "DE"
            };
            context.Delegations.Add(delgation);
            context.SaveChanges();
        }


    }


    public class CommitteeSeatOptions
    {
        public string CommitteeId { get; set; }

        public int CountryId { get; set; }

        public string SeatName { get; set; }
    }


    public class OrganizationOptionsBuilder
    {
        private MunityContext _context;

        public Organization Organization { get; }

        public OrganizationOptionsBuilder WithName(string name)
        {
            Organization.OrganizationName = name;
            return this;
        }

        public OrganizationOptionsBuilder WithShort(string shortName)
        {
            Organization.OrganizationShort = shortName;
            return this;
        }

        public OrganizationOptionsBuilder WithAdminRole()
        {
            var adminRole = new OrganizationRole();
            adminRole.Organization = this.Organization;
            this.Organization.Roles.Add(adminRole);
            adminRole.RoleName = "Admin";
            adminRole.CanCreateProject = true;
            adminRole.CanManageMembers = true;
            return this;
        }

        public OrganizationOptionsBuilder(MunityContext context)
        {
            this._context = context;
            this.Organization = new Organization();
        }

        public OrganizationOptionsBuilder WithProject(Action<ProjectOptionsBuilder> options)
        {
            var builder = new ProjectOptionsBuilder(this._context, this.Organization);
            options(builder);
            this.Organization.Projects.Add(builder.Project);
            return this;
        }

    }


    public class ProjectOptionsBuilder
    {
        public Project Project { get; }

        private readonly MunityContext _context;

        private bool _useEasyId = true;

        public ProjectOptionsBuilder WithName(string name)
        {
            this.Project.ProjectName = name;
            return this;
        }

        public ProjectOptionsBuilder WithShort(string shortName)
        {
            this.Project.ProjectShort = shortName;
            return this;
        }

        public ProjectOptionsBuilder WithCreationUser(MunityUser user)
        {
            this.Project.CreationUser = user;
            return this;
        }

        public ProjectOptionsBuilder WithOrganization(string organizationId)
        {
            var organization = _context.Organizations.FirstOrDefault(n => n.OrganizationId == organizationId);
            if (organization == null)
                throw new NullReferenceException("The Organization with the given Id cannot be found!");

            Project.ProjectOrganization = organization;
            return this;
        }

        public ProjectOptionsBuilder WithConference(Action<ConferenceOptionsBuilder> options)
        {
            var builder = new ConferenceOptionsBuilder(_context, Project);
            options(builder);
            var conference = builder.Conference;
            _context.Conferences.Add(conference);
            return this;
        }

        public ProjectOptionsBuilder(MunityContext context, Organization organization = null)
        {
            this.Project = new Project();
            this._context = context;
            this.Project.ProjectOrganization = organization;
        }
    }

    public class ConferenceOptionsBuilder
    {
        private Conference _conference;

        private readonly MunityContext _context;

        public Conference Conference
        {
            get
            {
                this._conference.CreationDate = DateTime.Now;
                return _conference;
            }
        }

        public ConferenceOptionsBuilder WithName(string name)
        {
            this._conference.Name = name;
            return this;
        }

        public ConferenceOptionsBuilder WithFullName(string fullName)
        {
            this._conference.FullName = fullName;
            return this;
        }

        public ConferenceOptionsBuilder WithShort(string shortName)
        {
            this._conference.ConferenceShort = shortName;
            return this;
        }

        public ConferenceOptionsBuilder WithStartDate(DateTime startDate)
        {
            this._conference.StartDate = startDate;
            return this;
        }

        public ConferenceOptionsBuilder WithEndDate(DateTime endDate)
        {
            this._conference.EndDate = endDate;
            return this;
        }

        public ConferenceOptionsBuilder WithBasePrice(decimal price)
        {
            this._conference.GeneralParticipationCost = price;
            return this;
        }

        public ConferenceOptionsBuilder ByUser(MunityUser user)
        {
            this._conference.CreationUser = user;
            return this;
        }

        public ConferenceOptionsBuilder WithProject(string projectId)
        {
            var project = _context.Projects.FirstOrDefault(n => n.ProjectId == projectId);
            if (project == null)
                throw new NullReferenceException($"The given Project {projectId} was not found. Make sure it exists.");

            Conference.ConferenceProject = project;
            return this;
        }

        public ConferenceOptionsBuilder WithCommittee(Action<CommitteeOptionsBuilder> builder)
        {
            var options = new CommitteeOptionsBuilder();
            builder(options);
            this._conference.Committees.Add(options.Committee);
            return this;
        }

        public ConferenceOptionsBuilder(MunityContext context, Project project = null)
        {
            this._conference = new Conference();
            this._context = context;
            this._conference.ConferenceProject = project;
        }
    }

    public class CommitteeOptionsBuilder
    {
        public Committee Committee { get; }

        public CommitteeOptionsBuilder WithName(string name)
        {
            Committee.Name = name;
            return this;
        }

        public CommitteeOptionsBuilder WithFullName(string fullName)
        {
            Committee.FullName = fullName;
            return this;
        }

        public CommitteeOptionsBuilder WithShort(string shortName)
        {
            Committee.CommitteeShort = shortName;
            return this;
        }

        public CommitteeOptionsBuilder WithTopic(Action<CommitteeTopicOptionsBuilder> options)
        {
            var builder = new CommitteeTopicOptionsBuilder();
            options(builder);
            Committee.Topics.Add(builder.Topic);
            return this;
        }

        public CommitteeOptionsBuilder WithTopic(string name)
        {
            return WithTopic(options => options.WithName(name));
        }

        public CommitteeOptionsBuilder WithSubCommittee(Action<CommitteeOptionsBuilder> options)
        {
            var builder = new CommitteeOptionsBuilder();
            options(builder);
            Committee.ChildCommittees.Add(builder.Committee);
            builder.Committee.ResolutlyCommittee = Committee;
            builder.Committee.Conference = this.Committee.Conference;
            return this;
        }

        public CommitteeOptionsBuilder()
        {
            this.Committee = new Committee();
        }
    }

    public class CommitteeTopicOptionsBuilder
    {
        public CommitteeTopic Topic { get; }

        public CommitteeTopicOptionsBuilder WithName(string name)
        {
            if (string.IsNullOrEmpty(Topic.TopicFullName))
                Topic.TopicFullName = name;

            Topic.TopicName = name;
            return this;
        }

        public CommitteeTopicOptionsBuilder WithFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(Topic.TopicName))
                Topic.TopicName = fullName;

            Topic.TopicFullName = fullName;
            return this;
        }

        public CommitteeTopicOptionsBuilder WithDescription(string description)
        {
            Topic.TopicDescription = description;
            return this;
        }

        public CommitteeTopicOptionsBuilder WithCode(string code)
        {
            Topic.TopicCode = code;
            return this;
        }

        public CommitteeTopicOptionsBuilder()
        {
            this.Topic = new CommitteeTopic();
        }
    }

    public class TeamRoleGroupBuilder : ITeamRoleBuilder, IRolesContainingTeamRoleBuilder
    {
        public TeamRoleGroup Group { get; }

        public ITeamRoleBuilder WithName(string name)
        {
            this.Group.Name = name;
            if (string.IsNullOrEmpty(Group.FullName))
                Group.FullName = name;
            return this;
        }

        public ITeamRoleBuilder WithFullName(string fullName)
        {
            this.Group.FullName = fullName;
            if (string.IsNullOrEmpty(Group.Name))
                Group.Name = fullName;
            return this;
        }

        public ITeamRoleBuilder WithShort(string shortName)
        {
            this.Group.TeamRoleGroupShort = shortName;
            return this;
        }

        public IRolesContainingTeamRoleBuilder WithRole(Action<TeamRoleBuilder> options)
        {
            var builder = new TeamRoleBuilder(Group);
            options(builder);
            Group.TeamRoles.Add(builder.Role);
            return this;
        }

        public IRolesContainingTeamRoleBuilder WithRole(string name, string shortName = "")
        {
            return WithRole(options => options.WithFullName(name)
                .WithName(name)
                .WithShort(shortName));
        }

        public IRolesContainingTeamRoleBuilder WithRoleLevel(int level)
        {
            foreach (var conferenceTeamRole in Group.TeamRoles)
            {
                conferenceTeamRole.TeamRoleLevel = level;
            }

            return this;
        }

        public IRolesContainingTeamRoleBuilder WithParentRole(ConferenceTeamRole role)
        {
            foreach (var conferenceTeamRole in Group.TeamRoles)
            {
                conferenceTeamRole.ParentTeamRole = role;
            }

            return this;
        }

        public TeamRoleGroupBuilder()
        {
            Group = new TeamRoleGroup();
        }
    }

    public interface ITeamRoleBuilder : ITeamRoleBuilderWithRole
    {
        ITeamRoleBuilder WithName(string name);

        ITeamRoleBuilder WithFullName(string fullName);

        ITeamRoleBuilder WithShort(string shortName);

    }

    public interface IRolesContainingTeamRoleBuilder : ITeamRoleBuilderWithRole
    {
        

        IRolesContainingTeamRoleBuilder WithRoleLevel(int level);

        IRolesContainingTeamRoleBuilder WithParentRole(ConferenceTeamRole role);
    }

    public interface ITeamRoleBuilderWithRole
    {
        IRolesContainingTeamRoleBuilder WithRole(Action<TeamRoleBuilder> options);

        IRolesContainingTeamRoleBuilder WithRole(string name, string shortName = "");
    }

    public class TeamRoleBuilder
    {
        public ConferenceTeamRole Role { get; }

        public TeamRoleBuilder WithName(string name)
        {
            Role.RoleName = name;
            return this;
        }

        public TeamRoleBuilder WithFullName(string fullName)
        {
            Role.RoleFullName = fullName;
            return this;
        }

        public TeamRoleBuilder WithShort(string shortName)
        {
            Role.RoleShort = shortName;
            return this;
        }

        public TeamRoleBuilder WithLevel(int level)
        {
            Role.TeamRoleLevel = level;
            return this;
        }

        public TeamRoleBuilder WithParent(ConferenceTeamRole role)
        {
            Role.ParentTeamRole = role;
            return this;
        }

        public TeamRoleBuilder(TeamRoleGroup group = null)
        {
            this.Role = new ConferenceTeamRole();
            Role.TeamRoleGroup = group;
        }
    }
}
