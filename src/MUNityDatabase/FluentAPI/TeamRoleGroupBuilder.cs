using System;
using MUNity.Database.Models.Conference;
using MUNity.Database.Models.Conference.Roles;

namespace MUNity.Database.FluentAPI
{
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
}
