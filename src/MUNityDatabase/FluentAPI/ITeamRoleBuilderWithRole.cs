using System;

namespace MUNity.Database.FluentAPI
{
    public interface ITeamRoleBuilderWithRole
    {
        IRolesContainingTeamRoleBuilder WithRole(Action<TeamRoleBuilder> options);

        IRolesContainingTeamRoleBuilder WithRole(string name, string shortName = "");
    }
}
