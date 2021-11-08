## Create a new Organization

```c#
var organization = new Organization()
{
    // The full name of the organization (max 150 chars)
    OrganizationName = "Deutsche Model United Nations e.V.",
    // A short Name for the organization (max 18 chars)
    OrganizationShort = "DMUN e.V."
};
context.Organizations.Add(organization);
context.SaveChanges();
```

The OrganizationId is a string and will be generated from GUID when a new instance is created. The Id can be changed to any other form as needed.

## Add a role to the organization

```c#
var role = new OrganizationRole()
{
    CanCreateProject = true,
    Organization = TestOrganization,
    RoleName = "Member"
};
_context.OrganizationRoles.Add(role);
_context.SaveChanges();
```

## Add a member to the organization

```c#
var membership = new OrganizationMember()
{
    JoinedDate = DateTime.Now,
    Organization = TestOrganization,
    Role = role,
    User = user
};
_context.OrganizationMember.Add(membership);
_context.SaveChanges();
```

## Get all users inside an organization

```c#
var users = _context.OrganizationMember.Where(n => n.Organization.OrganizationId == "ORGANIZATION_ID");
```

