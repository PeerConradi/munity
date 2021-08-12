# Conference Roles

Every time a user is participating in a conference it is with a certain Role. The key role types are TeamRole, SecretaryGeneralRole, DelegateRole and VisitorRole.

## Role Auth

Every role has an authorisation. These can be shared by multiple roles just make sure, that every role has an authorisation.

### Create a new RoleAuth

```C#
var leaderAuth = new RoleAuth()
{
    // Assign the authorization to a conference
    Conference = conference,
    // Set what this role is allowed to do
    CanEditConferenceSettings = true,
    CanEditParticipations = true,
    CanSeeApplications = true,
    // A power level with 1 being most powerful. This value can be used for external softwares that use the munity-api
    PowerLevel = 1,
    // The name of the role
    RoleAuthName = "Project-Owner"
};
context.RoleAuths.Add(leaderAuth);
```

## Team Roles

These roles should have the highest power inside the conference. The team can be organized in a hierarchy where you have the leader, something like a first level team and than the normal team members.

Team Roles can also be sorted into groups. 

### Example of creating a conference Leader Role

In this example a project-owner or project leader auth, group and one role is created.

```C#
// Create the auth for the leader
var leaderAuth = new RoleAuth()
{
    Conference = conference,
    CanEditConferenceSettings = true,
    CanEditParticipations = true,
    CanSeeApplications = true,
    PowerLevel = 1,
    RoleAuthName = "Project-Owner"
};
_context.RoleAuths.Add(leaderAuth);

// Create a group
var leaderRoleGroup = new TeamRoleGroup()
{
    FullName = "die Projektleitung",
    Name = "Projektleitung",
    TeamRoleGroupShort = "PL",
    GroupLevel = 1
};
_context.TeamRoleGroups.Add(leaderRoleGroup);

var leaderRole = new ConferenceTeamRole()
{
    Conference = conference,
    IconName = "pl",
    RoleFullName = "Projektleiter",
    RoleAuth = leaderAuth,
    RoleName = "Projektleiter",
    RoleShort = "PL",
    TeamRoleGroup = leaderRoleGroup,
    TeamRoleLevel = 1,
};

// You could create multiple leaderRoles if you want to have different Genders or types of leaders inside the group of project leaders.

_context.TeamRoles.Add(leaderRole);
_context.SaveChanges();
```

### Create a first Level Team

If you want to have a first level Team, for example the part of the team that is leading the sub-tasks you could create a group for that and add different types of roles into this. Add the project owner as the ParentRoleTeam if you want to create a hierarchy.

Example:
```C#
var firstLevelAuth = new RoleAuth()
{
    CanEditConferenceSettings = false,
    CanEditParticipations = false,
    CanSeeApplications = true,
    Conference = conference,
    PowerLevel = 2,
    RoleAuthName = "Erweiterte Projektleitung",
};

_context.RoleAuths.Add(firstLevelAuth);

var firstLevelGroup = new TeamRoleGroup()
{
    FullName = "Erweiterte Projektleitung",
    GroupLevel = 2,
    Name = "Erweiterte Projektleitung",
    TeamRoleGroupShort = "EPL"
};

_context.TeamRoleGroups.Add(firstLevelGroup);

var teamCoordinatorRole = new ConferenceTeamRole()
{
    // Set the conference
    Conference = conference,
    IconName = "un",
    // Set the project leader as the parent.
    ParentTeamRole = leaderRole,
    RoleAuth = firstLevelAuth,
    RoleFullName = "Team- und Materialkoordination",
    RoleName = "Team- und Materialkoordination",
    RoleShort = "TMK",
    TeamRoleGroup = firstLevelGroup,
    TeamRoleLevel = 2
};

var financeManager = new ConferenceTeamRole()
{
    Conference = conference,
    IconName = "un",
    ParentTeamRole = leaderRole,
    RoleAuth = firstLevelAuth,
    RoleFullName = "Finanzen",
    RoleName = "Finanzen",
    RoleShort = "CASH",
    TeamRoleGroup = firstLevelGroup,
    TeamRoleLevel = 2
};
```

