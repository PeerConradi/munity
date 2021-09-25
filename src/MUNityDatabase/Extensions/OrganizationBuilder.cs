using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using MUNity.Database.Context;
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
                builder.Group.TeamRoles.ForEach(n => n.Conference = conference);
            }
            return builder.Group;
        }

        public static Committee AddCommittee(this Conference conference, Action<CommitteeOptionsBuilder> options)
        {
            var builder = new CommitteeOptionsBuilder();
            options(builder);
            builder.Committee.Conference = conference;
            if (builder.Committee.ChildCommittees.Count > 0)
            {
                builder.Committee.ChildCommittees.ForEach(n => n.Conference = conference);
            }
            conference.Committees.Add(builder.Committee);
            return builder.Committee;
        }
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

        public OrganizationOptionsBuilder(MunityContext context)
        {
            this._context = context;
            this.Organization = new Organization();
        }

        public OrganizationOptionsBuilder WithProject(Action<ProjectOptionsBuilder> options)
        {
            var builder = new ProjectOptionsBuilder(this._context, this.Organization);
            options(builder);
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
