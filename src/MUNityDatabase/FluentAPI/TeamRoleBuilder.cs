using MUNity.Database.Models.Conference;
using MUNity.Database.Models.Conference.Roles;

namespace MUNity.Database.FluentAPI
{
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
