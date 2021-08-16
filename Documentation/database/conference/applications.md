# Application

## About

The application is the process of a user applying for a role inside a conference. This could be any type of role (team, delegate, press etc.).

Every role offers different types of applications that can be found under: [MUNityBase.EApplicationsStates.md](../../src/MUNityBase/../../../src/MUNityBase/EApplicationStates.cs).

The State of the application to a role is defined with the ApplicationState Property. This comes with the extra ApplicationValue property if you need to specify certain aspects.

## Application Types

The application types should show, how someone can apply to a specific role.

### Closed

This just means that the role doesn't allow any applications.

### DirectApplication

The user is applying directly for the Role. For Example there is one SecretaryGeneral Role created. The user can apply directly for this role. Same goes with any Role that is meant to be like this:

Use this when:
* The user applies for the role: Germany int he Assembly General.
* You have one Finance-Manager Role inside the Team that someone can apply on.

### Delegation Application

The user is appling for any Role inside the delegation, but doesn't care what type of role he/she gets.

Use this when:
* The user wants to be inside the delegation of Germany but doesn't care in what committee
* You have defiened a delegation with different role types (Delegate, Press etc.) and people can apply for any of those inside the delegation __without specifing which one it is going to be__

> While this setting can be selected inside Team-Roles it doesn't affect them.

This should create an instance of the type __DelegationApplication__ when an actual application is submitted.

#### Creating an Delegation Application

In this example we create a simple application on any delegation.

```c#
var delegationApplication = new DelegationApplication()
{
    Delegation = delegation,
    ApplyDate = DateTime.Now,
    User = user,
    Title = "Bewerbung auf Delegation Deutschland",
    Content = "Hallo, ich bin Max Mustermann und möchte mich auf die Delegation Deutschland bewerben."
};
_context.DelegationApplications.Add(delegationApplication);
_context.SaveChanges();
```

#### Get all delegation applications of a conference

This will return an IQueryable of every application that is pointed into a delegation (but not on a special role of a delegation.)

```c#
var delegationApplicationsOfConference =
    _context.DelegationApplications.Where(n => n.Delegation.Conference.ConferenceId == TestConference.ConferenceId);
```

### Apply in groups

You can specify that applications are only allowed in groups with ```EApplicationStates.OnlyDelegationGroup```

This will need the users to stack their applications inside a __GroupedRoleApplication.__

Example:

```c#
// Create a role Application
var applicationOne = new RoleApplication()
{
    ApplyDate = DateTime.Now,
    Content = "BLOB",
    Role = roleOne,
    Title = "Title",
    User = userOne
};

_context.RoleApplications.Add(applicationOne);

// Create another role application
var applicationTwo = new RoleApplication()
{
    ApplyDate = DateTime.Now,
    Content = "BLOB",
    Role = roleTwo,
    Title = "Title",
    User = userTwo
};

_context.RoleApplications.Add(applicationTwo);

// Add both to a list
var list = new List<RoleApplication>();
list.Add(applicationOne);
list.Add(applicationTwo);

// create a group
var group = new GroupedRoleApplication()
{
    Applications = list,
    CreateTime = DateTime.Now,
    GroupName = "Hello World"
};
_context.GroupedRoleApplications.Add(group);
_context.SaveChanges();
```



## Getting all applications of a conference

There are now two different types of applications inside the list:

The RoleApplications and the DelegationApplicaitons.

Role Applications can also be inside of groups (of GroupedRoleApplicaiton).