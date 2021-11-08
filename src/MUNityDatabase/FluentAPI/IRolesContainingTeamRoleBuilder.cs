using MUNity.Database.Models.Conference.Roles;

namespace MUNity.Database.FluentAPI;

public interface IRolesContainingTeamRoleBuilder : ITeamRoleBuilderWithRole
{


    IRolesContainingTeamRoleBuilder WithRoleLevel(int level);

    IRolesContainingTeamRoleBuilder WithParentRole(ConferenceTeamRole role);
}
