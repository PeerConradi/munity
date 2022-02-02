using MUNity.Database.FluentAPI;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.Test.MUNBW22Tests;

public partial class FullDMUN2022Tests
{
    [Test]
    [Order(100)]
    public void TestAddDMUNOrganization()
    {
        var orga = _context.Fluent.Organization.AddOrganization(options =>
            options.WithName("Deutsche Model United Nations e.V.")
                .WithShort("DMUN e.V.")
                .WithAdminRole()
                .WithMemberRole("Mitglied"));
        // Assert that the organization exists
        Assert.NotNull(orga);
        Assert.AreEqual(1, _context.Organizations.Count());
        Assert.IsTrue(_context.Organizations.Any(n => n.OrganizationId == "dmunev"));

        // Assert that the roles exist
        Assert.AreEqual(2, _context.OrganizationRoles.Count(n => n.Organization.OrganizationId == "dmunev"));
    }

    [Test]
    [Order(101)]
    public void TestMakeUserTheOrganizationAdmin()
    {
        var membership = _context.Fluent.ForOrganization("dmunev").AddUserIntoRole("tonystark", "Admin");
        Assert.NotNull(membership);
        Assert.AreEqual(1, _context.OrganizationMembers.Count());
    }

    [Test]
    [Order(103)]
    public void TestAddUsersToOrganizationAsMembers()
    {
        _context.Fluent.ForOrganization("dmunev").AddUserIntoRole("pparker", "Mitglied");
        Assert.AreEqual(2, _context.OrganizationMembers.Count());
        Assert.IsTrue(_context.Fluent.ForOrganization("dmunev").HasUserMembership("pparker"));
    }

    [Test]
    [Order(104)]
    public void TestAddUnknownUserThrowsException()
    {
        Assert.Throws<UserNotFoundException>(() => _context.Fluent.ForOrganization("dmunev").AddUserIntoRole("unknownUser", "Admin"));
    }

    [Test]
    [Order(105)]
    public void TestAddUserUnknowRoleThrowsException()
    {
        Assert.Throws<OrganizationRoleNotFoundException>(() => _context.Fluent.ForOrganization("dmunev").AddUserIntoRole("blackwidow", "NO ROLE"));
    }
}
