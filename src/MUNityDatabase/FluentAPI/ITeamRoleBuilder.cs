namespace MUNity.Database.FluentAPI
{
    public interface ITeamRoleBuilder : ITeamRoleBuilderWithRole
    {
        ITeamRoleBuilder WithName(string name);

        ITeamRoleBuilder WithFullName(string fullName);

        ITeamRoleBuilder WithShort(string shortName);

    }
}
